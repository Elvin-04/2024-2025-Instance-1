using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Save : MonoBehaviour
{
    public string path { get; private set; }
    public SaveObject obj;
    private void Awake()
    {
        path = Application.persistentDataPath + "/fffff.json";
        string directoryPath = Path.GetDirectoryName(path);
        if(!File.Exists(path))
        {
            string rawData = JsonUtility.ToJson(new Data());
            System.IO.File.WriteAllText(path, rawData);
        }

    }

    private void Start()
    {
        EventManager.instance.onScoreUpdated.AddListener((float score) => 
        {
            obj.score = 40;
            SaveToJson(obj); 
        });
        obj = LoadFromJson(obj.id);
    }
    public void SaveToJson(SaveObject obj)
    {
        Data data = GetData();

        if (data == null)
        {
            data = new Data();
        }

        data.ChangeData(obj);
        string rawData = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(path, rawData);
        Debug.Log($"Object is save :: ID :: {obj.id}, Score::{obj.score}");
    }

    public SaveObject LoadFromJson(int id)
    {
        Data data = GetData();
        if (data == null)
            return new SaveObject(id);
        SaveObject saveObject = data.GetObject(id);
        if (saveObject == null)
            return new SaveObject(id);
        Debug.Log($"Object is Load :: ID :: {saveObject.id}, Score::{saveObject.score}");
        return saveObject;
    }

    private Data GetData()
    {
        string rawData = System.IO.File.ReadAllText(path);
        return JsonUtility.FromJson<Data>(rawData);
    }
}

/// <summary>
/// Dictionnary with the id of a level in key and All the information of a level in value. 
/// This is the object we save 
/// </summary>
[System.Serializable]
public class Data
{

    public List<SaveObject> saveObjects = new(); // Replace Dictionary with List

    public Data() { }

    public void ChangeData(SaveObject obj)
    {
        // Remove existing object with the same ID
        saveObjects.RemoveAll(o => o.id == obj.id);
        // Add the new/updated object
        saveObjects.Add(obj);
    }

    public SaveObject GetObject(int id)
    {
        return saveObjects.FirstOrDefault(o => o.id == id);
    }
}

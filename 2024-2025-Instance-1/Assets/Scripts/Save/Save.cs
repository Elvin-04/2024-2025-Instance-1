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
        path = Application.persistentDataPath + "/LevelSaveInformation.json";
        string directoryPath = Path.GetDirectoryName(path);
        if(!File.Exists(path))
        {
            string rawData = JsonUtility.ToJson(new Data());
            File.WriteAllText(path, rawData);
        }

    }

    private void Start()
    {
        EventManager.instance.onScoreUpdated.AddListener((float stars) => 
        {
            obj.score = stars;
            SaveToJson(obj); 
        });
    }
    public void SaveToJson(SaveObject obj)
    {
        Data data = GetData();

        if (data == null)
        {
            data = new Data();
        }

        data.UdpadeBestScore(obj);
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
    public List<SaveObject> saveObjects = new();

    public Data() { }

    public void UdpadeBestScore(SaveObject obj)
    {
        SaveObject oldObject = saveObjects.FirstOrDefault(o => o.id == obj.id);
        oldObject ??= new SaveObject(obj.id);
        if (oldObject.score > obj.score)
            return;

        saveObjects.RemoveAll(o => o.id == obj.id);
        saveObjects.Add(obj);
    }

    public SaveObject GetObject(int id)
    {
        return saveObjects.FirstOrDefault(o => o.id == id);
    }
}

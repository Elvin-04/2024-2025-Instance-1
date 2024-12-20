
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;

public class Save
{
    public string path { get; private set; }
    public SaveObject obj;
    public Save()
    {
        path = Application.persistentDataPath + "/LevelSaveInformation.json";
        string directoryPath = Path.GetDirectoryName(path);
        if (!File.Exists(path))
        {
            string rawData = JsonUtility.ToJson(new Data());
            File.WriteAllText(path, rawData);
        }


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

    }

    public SaveObject LoadFromJson(int id)
    {
        Data data = GetData();
        if (data == null)
            return new SaveObject(id, -1);
        SaveObject saveObject = data.GetObject(id);
        if (saveObject == null)
            return new SaveObject(id, -1);

        return saveObject;
    }

    private Data GetData()
    {
         string rawData = System.IO.File.ReadAllText(path);
        return JsonUtility.FromJson<Data>(rawData);
    }
}

/// <summary> 
/// Contain the ListOf Object we should save 
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


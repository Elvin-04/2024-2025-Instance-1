using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using UnityEngine;
using static UnityEngine.Rendering.GPUSort;

public class Save : MonoBehaviour
{
    public string path { get; private set; }
    public SaveObject obj;
    private void Awake()
    {
        path = Application.persistentDataPath + "/Save.json";
        string directoryPath = Path.GetDirectoryName(path);
        try
        {
            if (!File.Exists(directoryPath))
            {
                Debug.LogWarning("Le fichier de sauvegarde n'existe pas encore.");
                System.IO.Directory.CreateDirectory(directoryPath);
            }
            Debug.Log("Données chargées avec succès.");
        }
        catch (UnauthorizedAccessException ex)
        {
            Debug.LogError("Accès non autorisé : " + ex.Message);
        }
        catch (Exception ex)
        {
            Debug.LogError("Erreur lors du chargement des données : " + ex.Message);
        }
        if (System.IO.File.Exists(path))
        {
            System.IO.FileInfo fileInfo = new FileInfo(path);
            fileInfo.IsReadOnly = false;
        }
    }

    private void Start()
    {
        EventManager.instance.onWin.AddListener(() => SaveToJson(obj));
        LoadFromJson(0);


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
        Debug.Log("Object is save");
    }

    public SaveObject LoadFromJson(int id)
    {
        Data data = GetData();
        if (data == null)
            return null;
        SaveObject saveObject = data.GetObject(id);
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

    private Dictionary<int, SaveObject> saveObjects = new();
    public Data(Dictionary<int, SaveObject> objects)
    {
        saveObjects = objects;
    }
    public Data()
    {
    }
    public void ChangeData(SaveObject obj)
    {
        saveObjects[obj.id] = obj;
    }
    public SaveObject GetObject(int id)
    {
        return saveObjects[id];
    }
}

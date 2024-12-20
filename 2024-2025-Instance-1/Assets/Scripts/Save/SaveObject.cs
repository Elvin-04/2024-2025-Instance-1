using UnityEngine;

/// <summary>
/// Contains all information you wanna save on a level
/// </summary>

[System.Serializable]
public class SaveObject
{
    public int score;
    public int id;


    public SaveObject(int id)
    {
        this.id = id;
    }

    public SaveObject(int id, int score)
    {
        this.id = id;
        this.score = score;
    }
}

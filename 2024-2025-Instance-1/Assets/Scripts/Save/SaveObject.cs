using UnityEngine;

/// <summary>
/// Contains all information you wanna save on a level
/// </summary>

[System.Serializable]
public class SaveObject : MonoBehaviour
{
    public float score;
    public int id;

    private void Start()
    {
        EventManager.instance.onScoreUpdated.AddListener((float newScore) => score = newScore);
    }
}

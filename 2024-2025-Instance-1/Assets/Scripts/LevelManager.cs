using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [field: SerializeField] public Transform spawnPoint { get; private set; }

    public UnityEvent onWin { get; private set; }

    private void Start()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        } 

        _instance = this;
    }

    public void Retry()
    {
        Debug.Log("Retry Level");
    }



    public void Win()
    {
        onWin.Invoke();
    }
}

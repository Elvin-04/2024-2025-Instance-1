using System.Collections;
using UnityEngine;
using UnityEngine.Events;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    public UnityEvent onWin;

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

    public void OnDeath()
    {

    }
}

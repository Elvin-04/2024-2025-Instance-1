using System;
using System.Collections;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

public class LevelManager : MonoBehaviour
{
    private static LevelManager _instance;

    [field: SerializeField] public Transform spawnPoint { get; private set; }

    public UnityEvent onWin { get; private set; }

    private void Awake()
    {
        Assert.IsNotNull(spawnPoint, "spawnPoint is null in LevelManager");
    }

    private void Start()
    {
        if (_instance)
        {
            Destroy(gameObject);
            return;
        } 

        _instance = this;
        EventManager.Instance.OnRetry.AddListener(ReloadScene);
    }

    public void Retry()
    {
        Debug.Log("Retry Level");
    }



    public void Win()
    {
        onWin.Invoke();
    }
    private void ReloadScene()
    {
        //print("Reloadddddding !!!!!!");
        Scene scene = SceneManager.GetActiveScene();
        SceneManager.LoadSceneAsync(scene.name);
    }

    ///////////////////////////////////////////////////////////////////
    //private void ReloadScene()
    //{
    //    StartCoroutine(LoadAsyncScene());
    //}
    //IEnumerator LoadAsyncScene()
    //{
    //    AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(SceneManager.GetActiveScene().name);

    //    while (!asyncLoad.isDone)
    //    {
    //        yield return null;
    //    }
    //}
    /////////////////////////////////////////////////////////////////////
}

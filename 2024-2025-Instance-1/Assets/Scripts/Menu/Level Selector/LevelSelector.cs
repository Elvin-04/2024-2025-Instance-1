using UnityEngine;
using System;
using UnityEngine.SceneManagement;

public class LevelSelector : MonoBehaviour
{
    public static LevelSelector instance {get; private set;}
    private LevelInfo _currentLevel;

    private void Start()
    {
        if (instance)
        {
            Destroy(gameObject);
            return;
        }

        LevelInfo.completedLevels.Clear();

        instance = this;
        DontDestroyOnLoad(gameObject);
    }

    public void StartLevel(LevelInfo level)
    {
        _currentLevel = level;
        SceneManager.LoadScene(level.levelScene);

        StartCoroutine(Utils.InvokeAfterUnscaled(() => EventManager.Instance.OnWin.AddListener(OnWin), 1.0f));
    }

    private void OnWin()
    {
        _currentLevel.MarkComplete();
    }
}
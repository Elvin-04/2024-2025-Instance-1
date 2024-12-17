using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseMenu : MonoBehaviour
{
    private bool _isOnPause = false;

    [SerializeField] private GameObject _pausePanel;
    private void Start()
    {
        EventManager.instance.onPause.AddListener(Pause);
        Time.timeScale = 1.0f;
    }
    public void ReturnMainMenu()
    {
        SceneManager.LoadScene("Test main menu");
    }
    public void Resume()
    {
        EventManager.instance.onEnableInput?.Invoke();
        _pausePanel.SetActive(false);
        Time.timeScale = 1.0f;
    }

    public void Quit()
    {
        Application.Quit();
    }
    private void Pause()
    {
        EventManager.instance.onDisableInput?.Invoke();
        _pausePanel.SetActive(true);
        Time.timeScale = 0f;
    }
}


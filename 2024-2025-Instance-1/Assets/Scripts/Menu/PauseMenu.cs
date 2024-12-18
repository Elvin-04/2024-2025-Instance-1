using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;
        private void Start()
        {
            EventManager.instance.onPause.AddListener(TogglePauseMenu);
            Time.timeScale = 1.0f;
        }
        public void ReturnMainMenu()
        {
            SceneManager.LoadScene(0);
        }
        private void Resume()
        {
            EventManager.instance.onEnableInput?.Invoke();
            _pausePanel.SetActive(false);
            Time.timeScale = 1.0f;
        }

        public void TogglePauseMenu()
        {
            if (_pausePanel.activeSelf)
            {
                Resume();
            }
            else
            {
                Pause();
            }
        }

        public void Quit()
        {
            Application.Quit();
        }
        private void Pause()
        {
            EventManager.instance.onDisableInput?.Invoke();
            _pausePanel.SetActive(true);
            //Time.timeScale = 0f;
        }
    }
}


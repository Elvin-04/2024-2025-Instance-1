using DG.Tweening;
using Player;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace Menu
{
    public class PauseMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _pausePanel;

        private void Start()
        {
            //EventManager.instance.onPause.AddListener(TogglePauseMenu);
            Time.timeScale = 1.0f;
        }

        public void ReturnMainMenu()
        {
            SceneManager.LoadScene("Test main menu");
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

            RectTransform menuTransform = GetComponent<RectTransform>();
            PlayerDirection direction = PlayerDirection.Right;

            RectTransform canvasRect = GetComponentInParent<Canvas>().GetComponent<RectTransform>();
            Vector2 targetPosition = direction switch
            {
                PlayerDirection.Up => new Vector2(0, canvasRect.rect.height / 2),
                PlayerDirection.Down => new Vector2(0, -canvasRect.rect.height / 2),
                PlayerDirection.Left => new Vector2(-canvasRect.rect.width / 2, 0),
                PlayerDirection.Right => new Vector2(canvasRect.rect.width / 2, 0),
                _ => new Vector2(canvasRect.rect.width / 2, canvasRect.rect.height / 2)
            };

            float duration = 1.0f;
            menuTransform.DOAnchorPos(targetPosition, duration);
        }

        private void Pause()
        {
            EventManager.instance.onDisableInput?.Invoke();
            _pausePanel.SetActive(true);
            Time.timeScale = 0f;
        }
    }
}
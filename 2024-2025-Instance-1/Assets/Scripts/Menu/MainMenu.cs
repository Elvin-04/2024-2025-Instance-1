using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuFirstBtn, _optionFirstBtn, _levelSelectionFirstBtn;
        public void Play()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void OpenOption()
        {
            EventSystem.current.SetSelectedGameObject(_optionFirstBtn);
        }

        public void GoToMainMenu()
        {
            EventSystem.current.SetSelectedGameObject(_mainMenuFirstBtn);
        }

        public void OpenLevelSelector()
        {
            EventSystem.current.SetSelectedGameObject(_levelSelectionFirstBtn);
        }

        public void Quit()
        {
            Application.Quit();
        }
    }
}
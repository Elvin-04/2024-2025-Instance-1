using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace Menu
{
    public class MainMenu : MonoBehaviour
    {
        [SerializeField] private GameObject _mainMenuFirstBtn, _optionFirstBtn;
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

        public void Quit()
        {
            Application.Quit();
        }
    }
}
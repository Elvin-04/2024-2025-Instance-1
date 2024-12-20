using System;
using Menu.Level_Selector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EndLevelUI : MonoBehaviour
    {
        [SerializeField] private int _NextLevel;
        [SerializeField] private GameObject _winPanelFirstBtn;
        public void QuitGame()
        {
            Application.Quit();
        }

        private void OnEnable()
        {
            EventSystem.current.SetSelectedGameObject(_winPanelFirstBtn);
        }

        public void RestartGame()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            PlayerPrefs.SetInt("ID", (PlayerPrefs.GetInt("ID")+1));
            SceneManager.LoadScene(_NextLevel);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
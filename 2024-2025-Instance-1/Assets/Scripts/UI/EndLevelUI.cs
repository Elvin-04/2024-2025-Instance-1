using System;
using Menu.Level_Selector;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EndLevelUI : MonoBehaviour
    {

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
            LevelSelector.instance.StartLevel(LevelSelector.instance.currentLevel);
        }

        public void NextLevel()
        {
            print(LevelSelector.instance == null);
            print(LevelSelector.instance.currentLevel == null);
            print(LevelSelector.instance.currentLevel.nextLevel == null);
            LevelSelector.instance.StartLevel(LevelSelector.instance.currentLevel.nextLevel);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("MainMenu");
        }
    }
}
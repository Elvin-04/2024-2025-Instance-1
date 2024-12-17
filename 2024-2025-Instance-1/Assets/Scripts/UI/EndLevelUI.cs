using Menu.Level_Selector;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace UI
{
    public class EndLevelUI : MonoBehaviour
    {
        public void QuitGame()
        {
            Application.Quit();
        }

        public void RestartGame()
        {
            LevelSelector.instance.StartLevel(LevelSelector.instance.currentLevel);
        }

        public void NextLevel()
        {
            LevelSelector.instance.StartLevel(LevelSelector.instance.currentLevel.nextLevel);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene("Ben Test main menu");
        }
    }
}
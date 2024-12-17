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
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
        }

        public void NextLevel()
        {
            SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex + 1);
        }

        public void MainMenu()
        {
            SceneManager.LoadScene(0);
        }
    }
}
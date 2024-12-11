using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    public void Play()
    {
        SceneManager.LoadScene("MVP");
        Debug.Log("Scene Play Load");
    }

    public void Quit()
    {
        Application.Quit();
        Debug.Log("Quit");
    }
}

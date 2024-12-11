using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;

public class LevelLoader : MonoBehaviour
{

    [SerializeField] private Animator _transition;

    [SerializeField] private float _transitionTime = 1f;

    [SerializeField] private GameObject _canva;

    [SerializeField] private bool _playFadeTransition = true;

    private void Start()
    {
        EventManager.Instance.OnTransitionScene.AddListener(LoadNextLevel);
        if (!_playFadeTransition)
        {
            _canva.SetActive(false);
        }
    }
    public void LoadNextLevel(string levelIndex)
    {
        _canva.SetActive(true);
        StartCoroutine(LoadLevel(levelIndex));
    }

    IEnumerator LoadLevel(string levelIndex)
    {
        _transition.SetTrigger("Start");
        yield return new WaitForSeconds(_transitionTime);
        SceneManager.LoadScene(levelIndex);
    }
}

using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] private Image _image;
    [SerializeField] private float _fadeDelay = 1f;

    private void Start()
    {
        DontDestroyOnLoad(this);
    }
    public void OpenPanel()
    {
        StartCoroutine(Fade(_image, _fadeDelay, true));
    }

    public void ClosePanel()
    {
        StartCoroutine(Fade(_image, _fadeDelay, false));
    }

    IEnumerator Fade(Image image, float fadeTime, bool fadeIn)
    {
        float elapsedTime = 0.0f;
        Color c = image.color;
        while (elapsedTime < fadeTime)
        {
            yield return null;
            elapsedTime += Time.deltaTime;
            if (fadeIn)
            {
                c.a = Mathf.Clamp01(elapsedTime / fadeTime);
            }
            else
            {
                c.a = 1f - Mathf.Clamp01(elapsedTime / fadeTime);
            }

            image.color = c;
        }
    }
}

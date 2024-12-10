using UnityEngine;
using UnityEngine.UI;

public class FadeTransition : MonoBehaviour
{
    [SerializeField] private Image _image;

    private bool _isAnimating = false;


    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        _image.color = new Color(1f, 1f, 1f, 0f);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}

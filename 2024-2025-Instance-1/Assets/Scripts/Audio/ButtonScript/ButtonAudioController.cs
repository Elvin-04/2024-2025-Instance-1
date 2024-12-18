using System.Collections;
using Managers.Audio;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class ButtonAudioController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private SoundsName _mouseOver;
    [SerializeField] private SoundsName _mouseClick;

    private Button _button;

    private bool _isSelected;
    //[Header("Gamepad")]

    private void Awake()
    {
        _button = GetComponent<Button>();
    }

    private void Start()
    {
        StartCoroutine(DelayedAddListener());
    }

    private void Update()
    {
        if (EventSystem.current.currentSelectedGameObject == gameObject && !_isSelected)
        {
            _isSelected = true;
            OnGamepadSelect();
        }
        else if (EventSystem.current.currentSelectedGameObject != gameObject && _isSelected)
        {
            _isSelected = false;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.instance.onPlaySfx?.Invoke(_mouseOver);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }

    private IEnumerator DelayedAddListener()
    {
        yield return new WaitForSeconds(5f);

        _button.onClick.AddListener(ButtonClick);
    }

    private void OnGamepadSelect()
    {
        EventManager.instance.onPlaySfx?.Invoke(_mouseOver);
    }

    private void ButtonClick()
    {
        EventManager.instance.onPlaySfx?.Invoke(_mouseClick);
    }
}
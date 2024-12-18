using Managers.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudioController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    [SerializeField] private SoundsName _mouseOver;
    [SerializeField] private SoundsName _mouseClick;

    public void OnPointerClick(PointerEventData eventData)
    {
        EventManager.instance.onPlaySfx?.Invoke(_mouseClick);
    }

    public void OnPointerEnter(PointerEventData eventData)
    {
        EventManager.instance.onPlaySfx?.Invoke(_mouseOver);
    }

    public void OnPointerExit(PointerEventData eventData)
    {
    }
}
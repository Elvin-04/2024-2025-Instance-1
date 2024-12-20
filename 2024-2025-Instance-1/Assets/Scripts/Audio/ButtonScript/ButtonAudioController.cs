using Managers.Audio;
using UnityEngine;
using UnityEngine.EventSystems;

public class ButtonAudioController : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler, IPointerClickHandler
{
    private readonly SoundsName _mouseClick = SoundsName.ButtonMouseClick;
    private readonly SoundsName _mouseOver = SoundsName.ButtonMouseOver;

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
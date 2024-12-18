using DG.Tweening;
using Player;
using UnityEngine;

public class AnimationMenu : MonoBehaviour
{
    [SerializeField] private RectTransform _transformBackground;

    [SerializeField] private float _durationAnim;

    [SerializeField] private PlayerDirection _direction;

    [SerializeField] private RectTransform _canvas;
    [SerializeField] private RectTransform _transformsButtonsTitle;


    [SerializeField] private float _ratio;

    //private void Awake()
    //{
    //    OffsetBackground();
    //    OffsetButtons();
    //}
    public void OffsetBackground()
    {
        _durationAnim = 1.0f;
        _transformBackground.DOAnchorPos(new Vector3(0,0,0), _durationAnim);
    }
    public void OffsetButtons()
    {
        Vector2 targetPosition = _direction switch
        {
            PlayerDirection.Up => new Vector2(0, _canvas.rect.height),
            PlayerDirection.Down => new Vector2(0, -_canvas.rect.height),
            PlayerDirection.Left => new Vector2(-_canvas.rect.width, 0),
            PlayerDirection.Right => new Vector2(_canvas.rect.width, 0),
            _ => new Vector2(_canvas.rect.width, _canvas.rect.height)
        };

        _transformsButtonsTitle.DOAnchorPos(targetPosition, _durationAnim);

    }
}

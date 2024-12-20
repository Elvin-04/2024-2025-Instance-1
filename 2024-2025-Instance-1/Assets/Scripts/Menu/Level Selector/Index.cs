using UnityEngine;
using UnityEngine.UI;

public class Index : MonoBehaviour
{
    [SerializeField] private Sprite _lockedSprite;
    [SerializeField] private Sprite _unlockedSprite;
    [SerializeField] private Sprite _SelectedSprite;

    //[SerializeField] private Color _Lockcolor;
    //[SerializeField] private Color _Unlockcolor;
    //[SerializeField] private Color _unlockcolorSelected;
    //[SerializeField] private Color _lockSelected;

    [SerializeField] private Image _image;

    public void ChangeColor(IndexState state)
    {
        switch (state) 
        {
            case IndexState.Lock: _image.sprite = _lockedSprite; break;
            case IndexState.Unlock: _image.sprite = _unlockedSprite; break;
            case IndexState.UnlockSelected: _image.sprite = _SelectedSprite;  break;
            case IndexState.LockSelected: _image.sprite = _SelectedSprite; break;
        }
    }

}

public enum IndexState
{
    Lock, Unlock, UnlockSelected, LockSelected
}
using UnityEngine;
using UnityEngine.UI;

public class Index : MonoBehaviour
{
    [SerializeField] private Color _Lockcolor;
    [SerializeField] private Color _Unlockcolor;
    [SerializeField] private Color _unlockcolorSelected;
    [SerializeField] private Color _lockSelected;

    [SerializeField] private Image _image;

    public void ChangeColor(IndexState state)
    {
        switch (state) 
        {
            case IndexState.Lock: _image.color = _Lockcolor; break;
            case IndexState.Unlock: _image.color = _Unlockcolor; break;
            case IndexState.UnlockSelected: _image.color = _unlockcolorSelected;  break;
            case IndexState.LockSelected: _image.color = _lockSelected; break;
        }
    }

}

public enum IndexState
{
    Lock, Unlock, UnlockSelected, LockSelected
}
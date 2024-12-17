using UnityEngine;

[RequireComponent(typeof(Animator))]
public class PlayerZoneEffect : MonoBehaviour
{
    private readonly string _removeZone = "RemoveZone";
    private readonly string _showZone = "ShowZone";
    private Animator _animator;
    private bool _isActive = false;

    private void Start()
    {
        _animator = GetComponent<Animator>();
        EventManager.instance.OnZoneEffect.AddListener(Show);
        EventManager.instance.StopZoneEffect.AddListener(Remove);
    }

    private void Show(int radius)
    {
        if (_isActive)
            return;
        transform.localScale = new Vector3(2 * (radius+1) - 1, 2 * (radius + 1) - 1, 2 * (radius + 1) - 1);
        _animator.Play(_showZone);
        _isActive = true;
    }

    private void Remove()
    {
        if (!_isActive)
            return;

        _animator.Play(_removeZone);
        _isActive = false;
    }
}

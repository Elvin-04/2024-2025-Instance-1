using Grid;
using UnityEngine;

public class CreateSpikeTrap : MonoBehaviour
{
    [SerializeField] private Cell _Active;
    [SerializeField] private Cell _Inactive;
    [SerializeField, Range(0,10)] private int _tick ;
    private int _numberOfTick = 0;

    private bool _isActive;

    private void Start()
    {
        Invoke(nameof(LateStart),0);
        EventManager.Instance.OnClockUpdated.AddListener(UpdateSpike);
    }

    private void LateStart()
    {
        SetTile(_isActive);
    }

    public void UpdateSpike()
    {
        _numberOfTick++;
        if (_tick <= _numberOfTick)
        {
            _isActive = true;
            _numberOfTick = 0;
        }
        else
        {
            _isActive = false;
        }
        SetTile(_isActive);
    }

    private Cell GetTile(bool isActive)
    {
        return isActive ? _Active : _Inactive;
    }

    private void SetTile(bool isActive)
    {
        EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTile(isActive));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
    }
}
using Grid;
using System;
using System.Collections;
using UnityEngine;

public class CreateSpikeTrap : MonoBehaviour
{
    [SerializeField] private Cell _animationUp;
    [SerializeField] private Cell _animationDown;
    [SerializeField, Range(0,10)] private int _tick ;
    private int _numberOfTick = 0;

    private bool _isActive = false;

    private void Start()
    {
        Invoke(nameof(LateStart),0);


    }

    private Cell GetTile(bool isActive)
    {
        return isActive ? _animationUp : _animationDown;
    }

    private void LateStart()
    {
        EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTile(_isActive));
        EventManager.Instance.OnClockUpdated?.AddListener(UpdateSpike);
    }

    public void UpdateSpike()
    {
        _numberOfTick++;
        if (_tick <= _numberOfTick)
        {
            _isActive = true;
            _numberOfTick = 0;
            EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTile(_isActive));
            return;
        }
        if (!_isActive)
            return;
            
        _isActive = false;
        EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTile(_isActive));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
    }
}
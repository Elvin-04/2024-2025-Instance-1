using Grid;
using System;
using UnityEngine;

public enum Direction
{
    North = 0,
    West  = 1,
    South = 2,
    East  = 3,
}

public class ArrowTrap : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] public GridManager _gridManager;

    [SerializeField] private Direction _direction;
    [SerializeField] private GameObject _arrowPrefab;

    [SerializeField] [Range(1, 10)] private int _fireEveryTicks = 2;

    private int _currentTick = 0;

    private void Start()
    {
        _transform = transform;

        EventManager.Instance.UpdateClock.AddListener(UpdateClock);
    }

    private void UpdateClock()
    {
        if (++_currentTick == _fireEveryTicks)
        {
            _currentTick = 0;
            ShootArrow();
        }
    }

    private void ShootArrow()
    {
        GameObject arrowObject = Instantiate(_arrowPrefab);
        arrowObject.SetActive(true);
        
        Transform arrowTransform = arrowObject.transform;

        arrowTransform.position = _transform.position;
        arrowTransform.rotation = Quaternion.Euler(0, 0, (int) _direction * 90);

        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.gridManager = _gridManager;
        arrow.direction = arrowTransform.rotation * Vector2.up;
    }
}

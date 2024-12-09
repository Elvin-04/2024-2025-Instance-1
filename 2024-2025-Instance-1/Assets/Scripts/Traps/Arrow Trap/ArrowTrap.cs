using Grid;
using UnityEngine;

public enum Direction
{
    North = 0,
    West = 1,
    South = 2,
    East = 3
}

public class ArrowTrap : MonoBehaviour
{
    [SerializeField] public GridManager _gridManager;

    [SerializeField] private Direction _direction;
    [SerializeField] private GameObject _arrowPrefab;

    [SerializeField] [Range(1, 10)] private int _fireEveryTicks = 2;

    // private List<Arrow> _arrows = new List<Arrow>();

    private int _currentTick;
    private Transform _transform;

    private void Start()
    {
        _transform = transform;
        _transform.position = _gridManager.GetCellPos(_transform.position);

        EventManager.Instance.UpdateClock.AddListener(UpdateClock);
    }

    private void UpdateClock()
    {
        // foreach (Arrow arrow in _arrows)
        //     arrow.EnableMovement();

        if (++_currentTick == _fireEveryTicks)
        {
            _currentTick = 0;
            ShootArrow();
        }
    }

    private void ShootArrow()
    {
        GameObject arrowObject =
            Instantiate(_arrowPrefab, _gridManager.GetCellPos(_transform.position), Quaternion.identity);
        arrowObject.SetActive(true);

        Transform arrowTransform = arrowObject.transform;

        arrowTransform.position = _transform.position;

        Arrow arrow = arrowObject.GetComponent<Arrow>();
        arrow.SetDirection(_direction);
        arrow.gridManager = _gridManager;
    }
}
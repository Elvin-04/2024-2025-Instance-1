using DG.Tweening;
using UnityEngine;
using UnityEngine.Tilemaps;

public class PlayerController : MonoBehaviour
{
    //Components
    private Transform _transform;

    //Properties
    [SerializeField] private Tilemap _tileMap;
    [SerializeField] private float _speed;
    private Vector2 _moveDirection;
    private bool _canMove;
    private bool _reachedTargetCell = true;

    private void Awake()
    {
        _transform = transform;
        Vector3Int position = _tileMap.WorldToCell(_transform.position);
        _transform.position = _tileMap.GetCellCenterWorld(position);
    }

    private void Start()
    {
        EventManager.Instance.OnMoveStarted?.AddListener(StartMove);
        EventManager.Instance.OnMoveCanceled?.AddListener(StopMove);
    }

    private void Update()
    {
        TryMove();
    }

    private void TryMove()
    {
        if (!_canMove || !_reachedTargetCell)
        {
            return;
        }

        Move();
    }

    private void StartMove(Vector2 direction)
    {
        _canMove = true;
        _moveDirection = direction;
    }

    private void Move()
    {
        _reachedTargetCell = false;
        _transform.DOMove(
            _tileMap.GetCellCenterWorld(_tileMap.WorldToCell(_transform.position + (Vector3)_moveDirection)),
            1 / _speed).SetEase(Ease.Linear).OnComplete(() => { _reachedTargetCell = true; });
    }

    private void StopMove()
    {
        _canMove = false;
    }

    private void Interact()
    {
    }
}
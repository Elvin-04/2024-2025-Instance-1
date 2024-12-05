using DG.Tweening;
using Grid;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        //Components
        private Transform _transform;

        //Properties
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private float _speed;
        private Vector2 _moveDirection;
        private bool _canMove;
        private bool _reachedTargetCell = true;
        public PlayerDirection CurrentDirection { get; private set; }

        private void Awake()
        {
            _transform = transform;
            Vector3 position = _gridManager.GetTilePosition(_transform.position);
            _transform.position = position;
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
            Vector3 nextPos = _transform.position + (Vector3)_moveDirection;
            Cell nextCell = _gridManager.GetCell(nextPos);
            if (nextCell == null)
            {
                StopMove();
                return;
            }
        
            CellObjectBase nextCellObject = nextCell.ObjectOnCell;
        
            if (nextCellObject is ICollisionObject)
            {
                StopMove();
                return;
            }
            
            CurrentDirection = _moveDirection.x switch
            {
                > 0 => PlayerDirection.Right,
                < 0 => PlayerDirection.Left,
                _ => _moveDirection.y switch
                {
                    > 0 => PlayerDirection.Up,
                    < 0 => PlayerDirection.Down,
                    _ => CurrentDirection
                }
            };
        
            _reachedTargetCell = false;
        
            Vector3 position = _gridManager.GetTilePosition(nextPos);
        
            _transform.DOMove(
                position,
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
}
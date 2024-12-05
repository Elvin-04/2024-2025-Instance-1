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
        private IInteractable _interactable;
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
            EventManager.Instance.OnInteract?.AddListener(Interact);
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

        private void GetInteractableFrontOfMe(Vector3 dir)
        {
            Vector3 nextPos = _transform.position + dir;
            _interactable = _gridManager.GetCell(nextPos).ObjectOnCell as IInteractable;
            Debug.Log(_interactable);
        }

        private void Move()
        {
            Vector3 nextPos = _transform.position + (Vector3)_moveDirection;
            Cell nextCell = _gridManager.GetCell(nextPos);
            
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
            
            if (nextCell == null)
            {
                StopMove();
                return;
            }
        
            CellObjectBase nextCellObject = nextCell.ObjectOnCell;
        
            if (nextCellObject is ICollisionObject)
            {
                StopMove();
                GetInteractableFrontOfMe(_moveDirection);
                return;
            }
            
            
        
            _reachedTargetCell = false;
            EventManager.Instance.UpdateClock?.Invoke();
        
            Vector3 position = _gridManager.GetTilePosition(nextPos);
        
            _transform.DOMove(
                position,
                1 / _speed).SetEase(Ease.Linear).OnComplete(() => 
                { 
                    _reachedTargetCell = true;
                    GetInteractableFrontOfMe(_moveDirection);
                });
        }

        private void StopMove()
        {
            _canMove = false;
        }

        private void Interact()
        {
            if(_interactable==null) return;
            _interactable.Interact();
        }
    }
}
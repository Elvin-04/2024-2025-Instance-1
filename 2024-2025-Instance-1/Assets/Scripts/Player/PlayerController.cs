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
        private GridManager _gridManager;
        [SerializeField] private float _speed;
        private Vector2 _moveDirection;
        private bool _canMove;
        private bool _reachedTargetCell = true;
        private IInteractable _interactable;
        public PlayerDirection currentDirection { get; private set; }
        private Tween _currentMoveAnim;

        private void Awake()
        {
            _transform = transform;
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

        //Has to be called when new player is spawned
        public void SetGridManager(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        public void StopMoveAnim()
        {
            _currentMoveAnim?.Kill();
            _reachedTargetCell = true;
            GetInteractableFrontOfMe(_moveDirection);
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
            Cell nextCell = _gridManager.GetCell(nextPos);
            if (nextCell != null)
            {
                _interactable = nextCell.objectOnCell as IInteractable;
            }
        }

        private void Move()
        {
            Vector3 nextPos = _transform.position + (Vector3)_moveDirection;
            Cell nextCell = _gridManager.GetCell(nextPos);
            
            currentDirection = _moveDirection.x switch
            {
                > 0 => PlayerDirection.Right,
                < 0 => PlayerDirection.Left,
                _ => _moveDirection.y switch
                {
                    > 0 => PlayerDirection.Up,
                    < 0 => PlayerDirection.Down,
                    _ => currentDirection
                }
            };
            
            if (nextCell == null)
            {
                StopMove();
                return;
            }
        
            CellObjectBase nextCellObject = nextCell.objectOnCell;
        
            if (nextCellObject is ICollisionObject)
            {
                StopMove();
                GetInteractableFrontOfMe(_moveDirection);
                return;
            }
            
            
        
            _reachedTargetCell = false;
            EventManager.Instance.UpdateClock?.Invoke();
        
            Vector3 position = _gridManager.GetTilePosition(nextPos);
        
            _currentMoveAnim = _transform.DOMove(
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
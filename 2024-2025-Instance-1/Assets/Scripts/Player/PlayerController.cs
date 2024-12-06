using DG.Tweening;
using Grid;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        //Components
        private Transform _transform;

        //Properties
        private GridManager _gridManager;
        [SerializeField] private float _movementTime;

        private Vector2 _moveDirection;
        private bool _canMove;
        private bool _reachedTargetCell = true;
        public PlayerDirection currentDirection { get; private set; }
        private Tween _currentMoveAnim;

        private IInteractable _interactableInFront;
        private IInteractable _interactableUnder;

        public PlayerDirection CurrentDirection { get; private set; }


        public UnityEvent onWin;

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

        private void CheckInteraction(Vector3 dir)
        {
            GetInteractableFrontOfMe(dir);
            GetInteractableUnderMe();
        }

        private void GetInteractableUnderMe()
        {
            IInteractable interact = _gridManager.GetCell(_transform.position).objectOnCell as IInteractable;
            Debug.Log(_transform.position + "  :: Position");
            Debug.Log(_gridManager.GetCell(_transform.position).objectOnCell + "  :: Object");
            

            if (interact != _interactableUnder)
            {
                _interactableUnder?.StopInteract();
            }
            _interactableUnder = interact;
            _interactableUnder?.Interact();
        }

        private void GetInteractableFrontOfMe(Vector3 dir)
        {
            (int, int) nextIndex = ((int)(_transform.position + dir).x, (int)(_transform.position + dir).y);

            Cell nextCell = _gridManager.GetCell(nextIndex);

            if (nextCell != null)
            {
                _interactableInFront = nextCell.objectOnCell as IInteractable;
            }
            EventManager.Instance.CanInteract.Invoke(_interactableInFront != null);
        }

        private void Move()
        {
            Vector2Int cellIndex = _gridManager.GetCellIndex(_transform.position);
            int yMoveDir = Mathf.CeilToInt(Mathf.Abs(_moveDirection.y)) * (int)Mathf.Sign(_moveDirection.y);
            int xMoveDir = Mathf.CeilToInt(Mathf.Abs(_moveDirection.x)) * (int)Mathf.Sign(_moveDirection.x);
            (int, int) nextIndex = (cellIndex.x + xMoveDir, cellIndex.y + yMoveDir);
            
            Cell nextCell = _gridManager.GetCell(nextIndex);

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
                CheckInteraction(_moveDirection);
                return;
            }


            _reachedTargetCell = false;
            EventManager.Instance.UpdateClock?.Invoke();
            Vector3 position = _gridManager.GetCellPos(nextIndex);

            _currentMoveAnim = _transform.DOMove(
                position,
                _movementTime).SetEase(Ease.Linear).OnComplete(() => 
                { 
                    CheckInteraction(_moveDirection);
                    // wait one frame. this is to allow interactions to actually happen
                    StartCoroutine(Utils.InvokeAfterFrame(() => _reachedTargetCell = true));
                });
        }

        private void StopMove()
        {
            _canMove = false;
        }

        private void Interact()
        {
            if (_interactableInFront == null) return;
            _interactableInFront.Interact();
        }

    }
}
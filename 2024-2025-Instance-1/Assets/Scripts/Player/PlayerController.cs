using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Grid;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] private float _movementTime;


        public UnityEvent onWin;
        private bool _canMove;
        private Tween _currentMoveAnim;

        //Properties
        private GridManager _gridManager;

        private List<IInteractable> _interactablesInFront = new();
        private List<IInteractable> _interactablesUnder = new();
        private Vector3 _interactablesUnderPosition;

        private Vector2 _moveDirection;

        private bool _reachedTargetCell = true;

        //Components
        private Transform _transform;

        public PlayerDirection? bannedDirection;
        public PlayerDirection currentDirection { get; private set; }

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            EventManager.Instance.OnMoveStarted?.AddListener(StartMove);
            EventManager.Instance.OnMoveCanceled?.AddListener(StopMove);
            EventManager.Instance.OnInteract?.AddListener(Interact);
            EventManager.Instance.OnDeath?.AddListener(StopMoveAnim);
        }

        private void Update()
        {
            TryMove();

            //////////////////////////////////////////////////////////////////
            if (Input.GetKeyDown(KeyCode.K)) EventManager.Instance.OnDeath.Invoke();
            ///////////////////////////////////////////////////////////////////
        }

        //Has to be called when new player is spawned
        public void SetGridManager(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        public void StopMoveAnim()
        {
            _interactablesUnder.Clear();
            _interactablesInFront.Clear();
            _currentMoveAnim?.Kill();
            _canMove = false;
            _reachedTargetCell = true;
        }

        private void TryMove()
        {
            if (!_canMove || !_reachedTargetCell) return;

            Move();
        }

        private void StartMove(Vector2 direction)
        {
            _canMove = true;
            _moveDirection = direction;
        }

        private void CheckInteraction<T>(Vector3 dir) where T : IInteractable
        {
            GetInteractableFrontOfMe<T>(dir);
            GetInteractableUnderMe();
        }

        private void GetInteractableUnderMe()
        {
            List<IInteractable> interacts =
                _gridManager.GetObjectsOnCell(_transform.position)
                    .Select(cellObject => cellObject as IInteractable).Where(interactable => interactable != null)
                    .ToList();

            List<IInteractable> commonInteracts = interacts.Intersect(_interactablesUnder).ToList();

            _interactablesUnder.Except(commonInteracts).ToList()
                .ForEach(interact =>
                {
                    if (_gridManager.GetCellObjectsByType(_interactablesUnderPosition, out List<IWeight> weights))
                    {
                        return;
                    }
                    interact?.StopInteract();
                });

            _interactablesUnder = interacts;
            _interactablesUnderPosition = _transform.position;

            foreach (IInteractable interactable in _interactablesUnder.ToList())
            {
                interactable?.Interact();
            }
        }

        private void GetInteractableFrontOfMe<T>(Vector3 dir) where T : IInteractable
        {
            Vector2Int nextIndex = _gridManager.GetNextIndex(_transform.position, dir);


            Cell nextCell = _gridManager.GetCell(nextIndex);

            if (nextCell == null) return;


            _interactablesInFront =
                _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex))
                    .Select(objectOnCell => objectOnCell as IInteractable).Where(interactable => interactable != null)
                    .ToList();

            List<IMoving> movingObjectsInFront = _interactablesInFront.OfType<IMoving>().ToList();
            movingObjectsInFront.ForEach(movingObjectInFront =>
            {
                if (dir.Equals(-movingObjectInFront.direction))
                    movingObjectsInFront.OfType<T>().ToList()
                        .ForEach(interactable => interactable.Interact());
            });

            EventManager.Instance.CanInteract.Invoke(_interactablesInFront.OfType<IInteractableCallable>().Count() > 0);
        }

        private void Move()
        {
            Vector2Int cellIndex = _gridManager.GetCellIndex(_transform.position);
            (int, int) nextIndex = (cellIndex.x + (int)_moveDirection.x, cellIndex.y + (int)_moveDirection.y);

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
                Debug.Log("next cell is null");
                StopMove();
                return;
            }

            List<CellObjectBase> nextCellObjects = _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex));


            if (nextCellObjects.Any(objectOnCell => objectOnCell != null && objectOnCell is ICollisionObject))
            {
                CheckInteraction<IInteractable>(_moveDirection);
                StopMove();
                return;
            }


            _reachedTargetCell = false;
            EventManager.Instance.UpdateClock?.Invoke();
            Vector3 position = _gridManager.GetCellPos(nextIndex);

            _currentMoveAnim = _transform.DOMove(
                position,
                _movementTime).SetEase(Ease.Linear).OnComplete(() =>
            {
                CheckInteraction<IInteractableCallable>(_moveDirection);
                EventManager.Instance.OnPlayerMoved?.Invoke(_transform.position);
                // wait one frame. this is to allow interactions to actually happen
                StartCoroutine(Utils.InvokeAfterFrame(() => _reachedTargetCell = true));
            });
            GetInteractableFrontOfMe<IInteractable>(_moveDirection);
        }

        private void ReachTarget()
        {
            _reachedTargetCell = true;
        }

        public void BanDirection(PlayerDirection direction)
        {
            bannedDirection ??= direction;
        }

        private void StopMove()
        {
            _canMove = false;
        }

        private void Interact()
        {
            if (_interactablesInFront.Count == 0) return;
            _interactablesInFront.ForEach(objectInFront => objectInFront.Interact());
        }
    }
}
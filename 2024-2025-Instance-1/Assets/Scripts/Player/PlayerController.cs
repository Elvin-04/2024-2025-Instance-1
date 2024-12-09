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

        private Vector2 _moveDirection;

        private bool _reachedTargetCell = true;

        //Components
        private Transform _transform;
        public PlayerDirection currentDirection { get; private set; }

        public PlayerDirection? bannedDirection = null;

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
            if (!_canMove || !_reachedTargetCell) return;

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
            List<IInteractable> interacts =
                _gridManager.GetObjectsOnCell(_transform.position)
                    .Select(cellObject => cellObject as IInteractable).Where(interactable => interactable != null)
                    .ToList();

            Debug.Log(interacts.Count);


            List<IInteractable> commonInteracts = interacts.Intersect(_interactablesUnder).ToList();

            _interactablesUnder.Except(commonInteracts).ToList()
                .ForEach(interact => interact?.StopInteract());

            _interactablesUnder = interacts;

            _interactablesUnder.ForEach(interactable => interactable?.Interact(this));
        }

        private void GetInteractableFrontOfMe(Vector3 dir)
        {
            (int, int) nextIndex = ((int)(_transform.position + dir).x, (int)(_transform.position + dir).y);

            Cell nextCell = _gridManager.GetCell(nextIndex);

            if (nextCell == null) return;

            _interactablesInFront =
                _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex))
                    .Select(objectOnCell => objectOnCell as IInteractable).Where(interactable => interactable != null)
                    .ToList();

            EventManager.Instance.CanInteract.Invoke(_interactablesInFront.Count > 0);
        }

        private void Move()
        {
            Vector2Int cellIndex = _gridManager.GetCellIndex(_transform.position);
            (int, int) nextIndex = (cellIndex.x + (int) _moveDirection.x, cellIndex.y + (int) _moveDirection.y);

            Cell nextCell = _gridManager.GetCell(nextIndex);

            PlayerDirection oldDiretion = currentDirection;

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

            // if (bannedDirection != null)
            // {
            //     if (currentDirection == bannedDirection)
            //     {
            //         currentDirection = oldDiretion;
            //         StopMove();

            //         Debug.Log("can't move that way");
            //         return;
            //     }
            //     else if (currentDirection == ((PlayerDirection) bannedDirection).GetOpposite())
            //         bannedDirection = null;
            // }

            if (nextCell == null)
            {
                Debug.Log("next cell is null");
                StopMove();
                return;
            }

            List<CellObjectBase> nextCellObjects = _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex));

            if (nextCellObjects.Any(objectOnCell => objectOnCell != null && objectOnCell is ICollisionObject))
            {
                Debug.Log("Collision");
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
            _interactablesInFront.ForEach(objectInFront => objectInFront.Interact(this));
        }
    }
}
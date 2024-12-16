using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Grid;
using UnityEngine;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        private Animator _animator;
        private bool _canMove;
        private Tween _currentMoveAnim;

        private bool _dead;

        //Properties
        private GridManager _gridManager;

        private List<IInteractable> _interactablesInFront = new();
        private List<IInteractable> _interactablesUnder = new();
        private Vector3 _interactablesUnderPosition;

        private Vector2 _moveDirection;

        private bool _reachedTargetCell = true;

        //Components
        private Transform _transform;
        public PlayerDirection currentDirection { get; private set; }

        private void Awake()
        {
            _transform = transform;

            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            EventManager.instance.onMoveStarted?.AddListener(StartMove);
            EventManager.instance.onMoveCanceled?.AddListener(StopMove);
            EventManager.instance.onInteract?.AddListener(Interact);
            EventManager.instance.onDeath?.AddListener(StopMoveAnim);

            EventManager.instance.onDeath?.AddListener(() => _dead = true);
            EventManager.instance.onRespawn?.AddListener(() => _dead = false);
        }

        private void Update()
        {
            TryMove();

            //////////////////////////////////////////////////////////////////
            if (Input.GetKeyDown(KeyCode.K)) EventManager.instance.onDeath.Invoke();
            ///////////////////////////////////////////////////////////////////
        }

        public Vector2 MoveDirection()
        {
            return _moveDirection;
        }

        //Has to be called when new player is spawned
        public void SetGridManager(GridManager gridManager)
        {
            _gridManager = gridManager;
        }

        public GridManager GetGridManager()
        {
            return _gridManager;
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
            var interacts =
                _gridManager.GetObjectsOnCell(_transform.position)
                    .Select(cellObject => cellObject as IInteractable).Where(interactable => interactable != null)
                    .ToList();

            var commonInteracts = interacts.Intersect(_interactablesUnder).ToList();

            _interactablesUnder.Except(commonInteracts).ToList()
                .ForEach(interact =>
                {
                    if (_gridManager.GetCellObjectsByType(_interactablesUnderPosition, out List<IWeight> _)) return;
                    interact?.StopInteract();
                });

            _interactablesUnder = interacts;
            _interactablesUnderPosition = _transform.position;

            if (!_dead)
                foreach (var interactable in _interactablesUnder.ToList())
                    interactable?.Interact();
        }

        private void GetInteractableFrontOfMe<T>(Vector3 dir) where T : IInteractable
        {
            var nextIndex = _gridManager.GetNextIndex(_transform.position, dir);


            var nextCell = _gridManager.GetCell(nextIndex);

            if (nextCell == null) return;


            _interactablesInFront =
                _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex))
                    .Select(objectOnCell => objectOnCell as IInteractable).Where(interactable => interactable != null)
                    .ToList();

            var movingObjectsInFront = _interactablesInFront.OfType<IMoving>().ToList();
            movingObjectsInFront.ForEach(movingObjectInFront =>
            {
                if (dir.Equals(-movingObjectInFront.direction))
                    movingObjectsInFront.OfType<T>().ToList()
                        .ForEach(interactable => interactable.Interact());
            });

            var callable = _interactablesInFront.OfType<IInteractableCallable>().ToList();
            var isNotNull = callable.Count > 0;
            EventManager.instance.canInteract.Invoke(callable.Count > 0, isNotNull ? callable[0].showName : "");
        }

        private void Move()
        {
            var nextIndex = _gridManager.GetNextIndex(_transform.position, _moveDirection);

            var nextCell = _gridManager.GetCell(nextIndex);

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

            var nextCellObjects = _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex));

            foreach (var interactableInFront in
                     nextCellObjects.OfType<IInteractableInFront>().ToList())
                interactableInFront.Interact();

            nextCellObjects = _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex));

            if (nextCellObjects.Any(objectOnCell => objectOnCell != null && objectOnCell is ICollisionObject))
            {
                CheckInteraction<IInteractable>(_moveDirection);
                StopMove();
                return;
            }

            SetAnimation((int)currentDirection);

            _reachedTargetCell = false;

            EventManager.instance.onPlayerMoved?.Invoke(_transform.position);
            var position = _gridManager.GetCellPos(nextIndex);
            _currentMoveAnim = _transform.DOMove(
                position,
                _gridManager.GetGlobalMoveTime()).SetEase(Ease.Linear).OnComplete(() =>
            {
                CheckInteraction<IInteractableCallable>(_moveDirection);
                EventManager.instance.onPlayerFinishedMoving?.Invoke(_transform.position);

                _reachedTargetCell = true;
            });
            GetInteractableFrontOfMe<IInteractable>(_moveDirection);
        }

        private void StopMove()
        {
            SetAnimation(0);
            _canMove = false;
        }

        private void Interact()
        {
            if (_interactablesInFront.Count == 0) return;
            _interactablesInFront.ForEach(objectInFront => objectInFront.Interact());
        }

        private void SetAnimation(int value)
        {
            if (_animator?.GetInteger("direction") != value)
                _animator.SetInteger("direction", value);
        }
    }
}
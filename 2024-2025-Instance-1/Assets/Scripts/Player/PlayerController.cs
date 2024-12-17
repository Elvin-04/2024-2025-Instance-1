using System.Collections.Generic;
using System.Linq;
using DG.Tweening;
using Grid;
using Managers.Audio;
using UnityEngine;
using UnityEngine.Assertions;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        [SerializeField] [Range(0f, 0.5f)] private float _movementHoldTime = 0.2f;
        [field: SerializeField] public SpriteRenderer spriteRenderer { get; private set; }
        private readonly List<IInteractable> _interactablesInFront = new();
        private Animator _animator;
        private bool _canMove;
        private Tween _currentMoveAnim;

        private bool _dead;

        //Properties
        private GridManager _gridManager;
        private float _holdingFor;
        private float _holdTime;
        private List<IInteractable> _interactablesUnder = new();
        private Vector3 _interactablesUnderPosition;

        private Vector2 _moveDirection;

        private bool _reachedTargetCell = true;

        //Components
        private Transform _transform;
        public PlayerDirection currentDirection { get; private set; }
        public Vector2 moveDirection => _moveDirection;

        private void Awake()
        {
            Assert.IsNotNull(spriteRenderer, "sprite renderer is null in player controller");
            _transform = transform;

            _animator = GetComponent<Animator>();
        }

        private void Start()
        {
            EventManager.instance.onMoveStarted?.AddListener(StartMove);
            EventManager.instance.onMoveCanceled?.AddListener(StopMove);
            EventManager.instance.onInteract?.AddListener(Interact);
            EventManager.instance.onDeath?.AddListener(StopMoveAnim);

            EventManager.instance.onDeath?.AddListener(deathEffect =>
            {
                _dead = true;
                StopMove();
            });
            EventManager.instance.onRespawn?.AddListener(() => _dead = false);
        }

        private void Update()
        {
            TryMove();

            //////////////////////////////////////////////////////////////////
            if (Input.GetKeyDown(KeyCode.K)) EventManager.instance.onDeath.Invoke(true);
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

        public void StopMoveAnim(bool deathEffect)
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

            if ((_holdingFor += Time.deltaTime) >= _holdTime)
            {
                _holdingFor = 0f;
                Move();
            }
        }

        private void StartMove(Vector2 direction)
        {
            _canMove = true;
            _holdTime = _moveDirection == direction ? _movementHoldTime : 0f;
            _moveDirection = direction;
        }

        private void CheckInteraction<T>(Vector3 dir) where T : IInteractable
        {
            GetInteractableFrontOfMe<T>(dir);
            GetInteractableUnderMe();
        }

        private void GetInteractableUnderMe()
        {
            _gridManager.GetObjectsOnCell(_transform.position)
                .Select(cellObject => cellObject as IInteractable).Where(interactable => interactable != null)
                .ToList();

            List<IInteractable> interacts = new();
            foreach (CellObjectBase objectOnCell in _gridManager.GetObjectsOnCell(_transform.position))
                if (objectOnCell is IInteractable interactable)
                    interacts.Add(interactable);

            /*List<IInteractable> commonInteracts = interacts.Intersect(_interactablesUnder).ToList();*/

            List<IInteractable> commonInteracts = new();
            foreach (IInteractable interactable in _interactablesUnder)
                if (interacts.Contains(interactable))
                    commonInteracts.Add(interactable);

            foreach (IInteractable interactable in _interactablesUnder)
                if (!commonInteracts.Contains(interactable))
                {
                    if (_gridManager.GetCellObjectsByType(_interactablesUnderPosition, out List<IWeight> _)) return;
                    interactable?.StopInteract();
                }

            _interactablesUnder = interacts;
            _interactablesUnderPosition = _transform.position;

            if (!_dead)
                foreach (IInteractable interactable in _interactablesUnder.ToList())
                    interactable?.Interact();
        }

        private void GetInteractableFrontOfMe<T>(Vector3 dir) where T : IInteractable
        {
            Vector2Int nextIndex = _gridManager.GetNextIndex(_transform.position, dir);


            Cell nextCell = _gridManager.GetCell(nextIndex);

            if (nextCell == null) return;

            _interactablesInFront.Clear();
            foreach (CellObjectBase objectOnCell in _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex)))
                if (objectOnCell is IInteractable interactable)
                    _interactablesInFront.Add(interactable);

            List<IMoving> movingObjectsInFront = new();

            foreach (IInteractable interactable in _interactablesInFront)
                if (interactable is IMoving moving)
                    movingObjectsInFront.Add(moving);

            movingObjectsInFront.ForEach(movingObjectInFront =>
            {
                if (dir.Equals(-movingObjectInFront.direction))
                    foreach (IMoving moving in movingObjectsInFront)
                        if (moving is T interactable)
                            interactable.Interact();
            });

            List<IInteractableCallable> callable = new();

            foreach (IInteractable interactable in _interactablesInFront.ToList())
                if (interactable is IInteractableCallable interactableCallable)
                    callable.Add(interactableCallable);

            bool isNotNull = callable.Count > 0;
            EventManager.instance.canInteract.Invoke(callable.Count > 0, isNotNull ? callable[0].showName : "");
        }

        private void Move()
        {
            //_moveDirection = direction;
            Vector2Int nextIndex = _gridManager.GetNextIndex(_transform.position, _moveDirection);
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

            List<CellObjectBase> nextCellObjects = _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex));

            foreach (CellObjectBase interactableInFront in
                     nextCellObjects.ToList())
                if (interactableInFront is IInteractableInFront interactableInFront1)
                    interactableInFront1.Interact();

            nextCellObjects = _gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex));

            foreach (CellObjectBase objectOnCell in nextCellObjects.ToList())
                if (objectOnCell is ICollisionObject)
                {
                    CheckInteraction<IInteractable>(_moveDirection);
                    StopMove();
                    return;
                }

            SetAnimation((int)currentDirection);

            _reachedTargetCell = false;
            EventManager.instance.onPlaySfx?.Invoke(SoundsName.SandMovementPlayer);

            EventManager.instance.onPlayerMoved?.Invoke(_transform.position);
            Vector3 position = _gridManager.GetCellPos(nextIndex);
            _currentMoveAnim = _transform.DOMove(
                position,
                _gridManager.GetGlobalMoveTime()).SetEase(Ease.Linear).OnComplete(() =>
            {
                CheckInteraction<IInteractableCallable>(_moveDirection);
                EventManager.instance.onPlayerFinishedMoving?.Invoke(_transform.position);

                _reachedTargetCell = true;
                _holdTime = _movementHoldTime;
            });
            GetInteractableFrontOfMe<IInteractable>(_moveDirection);
        }

        private void StopMove()
        {
            SetAnimation(0);
            _canMove = false;
            _moveDirection = Vector2.zero;
            _holdTime = 0f;
            _holdingFor = 0f;
        }

        private void Interact()
        {
            if (_interactablesInFront.Count == 0) return;
            _interactablesInFront
                .Where(objectInFront => objectInFront.canPickUp && objectInFront is not IInteractableInFront).ToList()
                .ForEach(objectInFront => objectInFront.Interact());
        }

        private void SetAnimation(int value)
        {
            if (_animator?.GetInteger("direction") != value)
                if (_animator != null)
                    _animator.SetInteger("direction", value);
        }
    }
}
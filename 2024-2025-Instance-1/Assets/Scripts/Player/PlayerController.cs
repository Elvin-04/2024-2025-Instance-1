using DG.Tweening;
using Grid;
using System;
using UnityEngine;
using UnityEngine.Events;

namespace Player
{
    public class PlayerController : MonoBehaviour
    {
        //Components
        private Transform _transform;

        //Properties
        [SerializeField] private GridManager _gridManager;
        [SerializeField] private float _movementTime;

        private Vector2 _moveDirection;
        private bool _canMove;
        private bool _reachedTargetCell = true;
        private IInteractable _interactable;
        public PlayerDirection currentDirection { get; private set; }
        private Tween _currentMoveAnim;


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
            _interactable = _gridManager.GetCell(_transform.position).objectOnCell as IInteractable;
            _interactable?.Interact();
            Debug.Log(_interactable);
        }

        private void GetInteractableFrontOfMe(Vector3 dir)
        {
            Vector3 nextPos = _transform.position + dir;
            Cell nextCell = _gridManager.GetCell(nextPos);
            if (nextCell != null)
            {
                _interactable = nextCell.objectOnCell as IInteractable;
            }
            EventManager.Instance.CanInteract.Invoke(_interactable != null);
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
                CheckInteraction(_moveDirection);
                return;
            }
            
            
        
            _reachedTargetCell = false;
            EventManager.Instance.UpdateClock?.Invoke();
        
            Vector3 position = _gridManager.GetTilePosition(nextPos);
        
            _currentMoveAnim = _transform.DOMove(
                position,
                _movementTime).SetEase(Ease.Linear).OnComplete(() => 
                { 
                    _reachedTargetCell = true;
                    GetInteractableFrontOfMe(_moveDirection);

                    CheckInteraction(_moveDirection);
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





    private void OnTriggerEnter2D(Collider2D collider)
    {
        CellObjectBase cellObject = _gridManager.GetCell(_transform.position + (Vector3)_moveDirection).objectOnCell;

        Debug.Log(cellObject);

        if (cellObject is EnterNewRoomCell)
        {
            Vector2Int moveCamTo = ((EnterNewRoomCell) cellObject).nextCamPos;
            Camera.main.transform.DOMove(new Vector3(moveCamTo.x, moveCamTo.y, Camera.main.transform.position.z), 0.5f).SetEase(Ease.OutCubic);
        }
        else if (cellObject is WinCell)
        {
            onWin.Invoke();
        }

    }




    }
}
using System.Linq;
using DG.Tweening;
using Grid;
using Player;
using UnityEngine;

namespace Traps.Arrow_Trap
{
    public class Arrow : CellObjectBase, IInteractable, IMoving
    {
        public PlayerDirection directionEnum;
        public Vector2 direction;

        private Transform _transform;
        private GridManager _gridManager;

        private void Awake()
        {
            _transform = transform;
        }

        private void Start()
        {
            EventManager.instance.updateClock.AddListener(UpdateClock);
        }

        public bool canPickUp { get; set; }

        public void Interact()
        {
            EventManager.instance.onDeath?.Invoke();
        }

        public void StopInteract()
        {
        }

        Vector2 IMoving.direction => direction;

        public void SetGridManager(GridManager gridManagerToSet)
        {
            _gridManager = gridManagerToSet;
        }

        public void SetDirection(PlayerDirection directionToSet)
        {
            transform.rotation = Quaternion.Euler(0, 0, (int)(directionToSet + 1) * 90);

            directionEnum = directionToSet;
        }

        public void UpdateClock()
        {
            Vector2Int cellIndex = _gridManager.GetCellIndex(_transform.position);

            direction = directionEnum switch
            {
                PlayerDirection.Up => Vector2.up,
                PlayerDirection.Down => Vector2.down,
                PlayerDirection.Right => Vector2.right,
                PlayerDirection.Left => Vector2.left,
                _ => Vector3.zero
            };

            Vector2Int nextIndex = _gridManager.GetNextIndex(cellIndex, direction);

            if (_gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex)).OfType<ICollisionObject>().Any())
            {
                EventManager.instance.updateClock.RemoveListener(UpdateClock);
                Destroy(gameObject);
                _gridManager.RemoveObjectOnCell(cellIndex, this);
                return;
            }

            _transform.DOMove(_gridManager.GetCellPos(nextIndex), _gridManager.GetGlobalMoveTime()).SetEase(Ease.Linear).OnComplete(() =>
            {
                _gridManager.AddObjectOnCell(nextIndex, this);
                _gridManager.RemoveObjectOnCell(cellIndex, this);
            });
        }
    }
}
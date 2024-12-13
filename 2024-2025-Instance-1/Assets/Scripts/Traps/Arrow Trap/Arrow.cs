using System.Linq;
using DG.Tweening;
using Grid;
using Managers.Audio;
using Player;
using UnityEngine;

namespace Traps.Arrow_Trap
{
    public class Arrow : CellObjectBase, IInteractable, IMoving
    {
        public PlayerDirection directionEnum;
        public Vector2 direction;
        private GridManager _gridManager;

        private bool _movedOnce;

        private Tween _moveTween;

        private Transform _transform;

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
            EventManager.instance.onDeath?.Invoke(true);
            EventManager.instance.onPlaySfx?.Invoke(SoundsName.DeathByArrow, null);
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

            switch (directionToSet)
            {
                case PlayerDirection.Left: transform.rotation = Quaternion.Euler(0, 0, 180); break;
                case PlayerDirection.Up: transform.rotation = Quaternion.Euler(0, 0, 90); break;
                case PlayerDirection.Down: transform.rotation = Quaternion.Euler(0, 0, -90); break;
                case PlayerDirection.Right: transform.rotation = Quaternion.Euler(0, 0, 0); break;
            }

            directionEnum = directionToSet;
        }

        public void UpdateClock()
        {
            var cellIndex = _gridManager.GetCellIndex(_transform.position);

            direction = directionEnum switch
            {
                PlayerDirection.Up => Vector2.up,
                PlayerDirection.Down => Vector2.down,
                PlayerDirection.Right => Vector2.right,
                PlayerDirection.Left => Vector2.left,
                _ => Vector3.zero
            };

            var nextIndex = _gridManager.GetNextIndex(cellIndex, direction);

            if (_gridManager.GetObjectsOnCell(_gridManager.GetCellPos(nextIndex)).OfType<ICollisionObject>().Any() ||
                (_gridManager.GetObjectsOnCell(_gridManager.GetCellPos(cellIndex)).OfType<ICollisionObject>().Any() &&
                 _movedOnce))
            {
                EventManager.instance.updateClock.RemoveListener(UpdateClock);
                _moveTween?.Kill();
                EventManager.instance.onPlaySfx?.Invoke(SoundsName.ImpactArrowWithWall, transform);
                Destroy(gameObject);
                _gridManager.RemoveObjectOnCell(cellIndex, this);
                return;
            }

            _moveTween = transform.DOMove(_gridManager.GetCellPos(nextIndex), _gridManager.GetGlobalMoveTime())
                .SetEase(Ease.Linear)
                .OnComplete(() =>
                {
                    _gridManager.AddObjectOnCell(nextIndex, this);
                    _gridManager.RemoveObjectOnCell(cellIndex, this);
                });
            _movedOnce = true;
        }
    }
}
using Grid;
using Managers.Audio;
using Player;
using UnityEngine;
using UnityEngine.Assertions;

namespace Traps.Arrow_Trap
{
    public class ArrowTrap : MonoBehaviour
    {
        [SerializeField] private GridManager _gridManager;

        [SerializeField] private PlayerDirection _direction;
        [SerializeField] private GameObject _arrowPrefab;

        [SerializeField] [Range(1, 10)] private int _fireEveryTicks = 2;

        private int _currentTick;
        private Transform _transform;

        private void Start()
        {
            Assert.IsNotNull(_gridManager, "grid manager is null in ArrowTrap");
            Assert.IsNotNull(_arrowPrefab, "arrow prefab is null in ArrowTrap");

            _transform = transform;
            _transform.position = _gridManager.GetCellPos(_transform.position);

            EventManager.instance.updateClock.AddListener(UpdateClock);
        }

        private void UpdateClock()
        {
            if (++_currentTick != _fireEveryTicks) return;
            _currentTick = 0;
            ShootArrow();

            Debug.Log($"EventManager.instance: {EventManager.instance}");
            Debug.Log($"transform: {transform}");
            EventManager.instance.onPlaySfx?.Invoke(SoundsName.ActiveArrowThrower, transform);
        }

        private void ShootArrow()
        {
            var arrowObject =
                Instantiate(_arrowPrefab, _gridManager.GetCellPos(_transform.position), Quaternion.identity);
            arrowObject.SetActive(true);

            var arrowTransform = arrowObject.transform;

            arrowTransform.position = _transform.position;

            var arrow = arrowObject.GetComponent<Arrow>();
            arrow.SetDirection(_direction);
            arrow.SetGridManager(_gridManager);
        }
    }
}
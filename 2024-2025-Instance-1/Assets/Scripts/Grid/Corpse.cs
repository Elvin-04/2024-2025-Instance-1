using UnityEngine;

namespace Grid
{
    public class Corpse : CellObjectBase, IWeight
    {
        //Components
        [SerializeField] private int _lifeTime = 10;
        private int _currentLifeTime;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            _currentLifeTime = _lifeTime;
            EventManager.instance.onClockUpdated?.AddListener(UpdateTime);
        }

        private void UpdateTime()
        {
            _currentLifeTime--;
            if (_currentLifeTime > 0) return;

            EventManager.instance.onRemoveObjectOnCell?.Invoke(_transform.position, this);
            EventManager.instance.stopInteract?.Invoke(_transform.position);
            Destroy(gameObject);
        }
    }
}
using System;
using UnityEngine;

namespace Grid
{
    public class Corpse : CellObjectBase, IWeight
    {
        //Components
        [SerializeField] private int _lifeTime = 10;
        private int _currentLifeTime;
        private Transform _transform;
        public Action<int> onTick;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
            _currentLifeTime = _lifeTime;
        }

        private void Start()
        {
            EventManager.instance.onClockUpdated?.AddListener(UpdateTime);
        }

        public int GetCurrentLifeTime()
        {
            return _currentLifeTime;
        }

        private void UpdateTime()
        {
            onTick?.Invoke(--_currentLifeTime);
            if (_currentLifeTime > 0) return;

            EventManager.instance.onRemoveObjectOnCell?.Invoke(_transform.position, this);
            EventManager.instance.stopInteract?.Invoke(_transform.position);
            Destroy(gameObject);
        }
    }
}
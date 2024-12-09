using System;
using UnityEngine;

namespace Grid
{
    public class Corpse : CellObjectBase
    {
        //Components
        [SerializeField] private int _lifeTime = 10;
        private int _currentLifeTime = 0;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            _currentLifeTime = _lifeTime;
            EventManager.Instance.OnClockUpdated?.AddListener(UpdateTime);
        }

        private void UpdateTime()
        {
            _currentLifeTime--;
            if (_currentLifeTime <= 0)
            {
                Destroy(gameObject);
            }
        }
    }
}
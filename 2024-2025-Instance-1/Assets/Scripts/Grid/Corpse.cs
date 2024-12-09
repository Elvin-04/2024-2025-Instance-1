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
            EventManager.Instance.OnClockUpdated?.AddListener(UpdateTime);
        }

        private void UpdateTime()
        {
            _currentLifeTime--;
            if (_currentLifeTime > 0) return;

            EventManager.Instance.OnRemoveObjectOnCell?.Invoke(_transform.position, this);
            EventManager.Instance.StopInteract?.Invoke(_transform.position);
            Destroy(gameObject);
        }
    }
}
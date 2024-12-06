using UnityEngine;

namespace Grid
{
    public class Corpse : CellObjectBase
    {
        //Components
        [SerializeField] private float _lifeTime = 10f;
        private float _currentLifeTime;
        private Transform _transform;

        private void Awake()
        {
            _transform = GetComponent<Transform>();
        }

        private void Start()
        {
            _currentLifeTime = _lifeTime;
        }

        private void Update()
        {
            _currentLifeTime -= Time.deltaTime;
            if (_currentLifeTime <= 0f)
            {
                EventManager.Instance?.OnResetCell.Invoke(_transform.position);   
            }
        }
    }
}
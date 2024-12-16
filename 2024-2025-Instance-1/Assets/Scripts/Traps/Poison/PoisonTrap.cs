using Creators;
using Grid;
using UnityEngine;

namespace Traps
{
    public class PoisonTrap : CellObjectBase, IInteractable, IWeightInteractable
    {
        [SerializeField] private int _maxTickClock;


        [HideInInspector] public PoisonTrapCreator creator;
        private bool _playerPoisoned;

        public int currentLifeTime { get; private set; }
        public int maxLifeTime => _maxTickClock;

        private void Start()
        {
            EventManager.instance.onDeath?.AddListener(OnDeath);
            EventManager.instance.onClockUpdated?.AddListener(OnClockUpdate);
            currentLifeTime = _maxTickClock;
        }

        public bool canPickUp
        {
            get => false;
            set { }
        }

        public void Interact()
        {
            _playerPoisoned = true;
        }

        public void StopInteract()
        {
        }

        public void WeightInteract()
        {
            creator.WeightInteract(this);
        }

        public void StopWeightInteract()
        {
            creator.StopWeightInteract(this);
        }

        public void OnClockUpdate()
        {
            if (!_playerPoisoned)
                return;

            UpdateLifeTime();
            CheckPlayerDied();
        }

        private void UpdateLifeTime()
        {
            currentLifeTime--;
        }

        private void CheckPlayerDied()
        {
            if (currentLifeTime < 0) EventManager.instance.onDeath?.Invoke();
        }

        private void OnDeath()
        {
            currentLifeTime = _maxTickClock;
            _playerPoisoned = false;
        }
    }
}
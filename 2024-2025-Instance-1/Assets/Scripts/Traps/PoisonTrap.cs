using Grid;
using UnityEngine;

namespace Traps
{

    public class PoisonTrap : CellObjectBase, IInteractable
    {
        [SerializeField] private int _maxTickClock;
        private int _currentTickClock;
        public bool canPickUp { get => false; set { } }

        private void Start()
        {
            EventManager.instance.onDeath?.AddListener(OnDeath); 
        }
        public void OnClockUpdate()
        {
            _currentTickClock++;
            Debug.Log(_currentTickClock);

            if (_currentTickClock >= _maxTickClock)
            {
                EventManager.instance.onDeath?.Invoke();
                Debug.Log("Tu marche ?");
            }

        }

        public void Interact()
        {
            EventManager.instance.onClockUpdated?.AddListener(OnClockUpdate);
        }

        public void StopInteract()
        {
            
        }

        private void OnDeath()
        {
            _currentTickClock = 0;
            EventManager.instance.onClockUpdated?.RemoveListener(OnClockUpdate);
        }
    }
}

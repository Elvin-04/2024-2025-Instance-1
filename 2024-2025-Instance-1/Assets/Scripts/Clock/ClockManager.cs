using UnityEngine;

namespace Clock
{
    public class ClockManager : MonoBehaviour
    {
        private static ClockManager _instance;

        private void Awake()
        {
            if (_instance == null)
                _instance = this;
            else
                Destroy(gameObject);
        }

        private void Start()
        {
            EventManager.instance.updateClock.AddListener(UpdateClock);
        }

        public void UpdateClock()
        {
            EventManager.instance.onClockUpdated?.Invoke();
        }
    }
}
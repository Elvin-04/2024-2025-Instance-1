using System;
using UnityEngine;
using UnityEngine.Assertions;

namespace Clock
{
    public class ClockManager : MonoBehaviour
    {
        private static ClockManager _instance;

        private void Awake()
        {
            if (_instance == null)
            {
                _instance = this;
            }
            else
            {
                Destroy(this);
            }
        }

        private void Start()
        {
            if (EventManager.Instance != null)
            {
                EventManager.Instance.UpdateClock.AddListener(UpdateClock);
            }
            else
            {
                Debug.LogWarning("Event manager instance is null");
            }
        }

        public void UpdateClock()
        {
            EventManager.Instance.OnClockUpdated?.Invoke();
            Debug.Log("clock updated");
        }
    }
}
using Grid;
using UnityEngine;

namespace Creators
{
    public class CreateSpikeTrap : SwitchableCellCreator
    {
        [SerializeField, Range(0,10)] private int _tick ;
        private int _numberOfTick;

        private bool _isActive;

        protected override void Start()
        {
            base.Start();
            EventManager.Instance.OnClockUpdated.AddListener(UpdateSpike);
        }

        public void UpdateSpike()
        {
            _numberOfTick++;
            if (_tick <= _numberOfTick)
            {
                _isActive = true;
                _numberOfTick = 0;
            }
            else
            {
                _isActive = false;
            }
            SetTile(_isActive);
        }

        private void SetTile(bool isActive)
        {
            EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTileBasedOnState(isActive));
        }
    }
}
using Managers.Audio;

namespace Creators
{
    public class CreateSpikeTrap : SwitchableCellCreator
    {
        private bool _isActive;

        protected override void Start()
        {
            base.Start();
            EventManager.instance.onClockUpdated.AddListener(UpdateSpike);
        }

        public void UpdateSpike()
        {
            _numberOfTick++;
            if (_tick <= _numberOfTick)
            {
                _isActive = true;
                _numberOfTick = 0;
                EventManager.instance.onChangeCell?.Invoke(transform.position, GetTileBasedOnState(_isActive));
                EventManager.instance.onPlaySfx?.Invoke(SoundsName.ActivateSpike, transform);
                return;
            }

            if (!_isActive)
                return;

            _isActive = false;
            EventManager.instance.onPlaySfx?.Invoke(SoundsName.DeactivateSpike, transform);
            EventManager.instance.onChangeCell?.Invoke(transform.position, GetTileBasedOnState(_isActive));
        }

        private void SetTile(bool isActive)
        {
            EventManager.instance.onChangeCell?.Invoke(transform.position, GetTileBasedOnState(isActive));
        }
    }
}
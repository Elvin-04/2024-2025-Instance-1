using Grid;
using Managers.Audio;

namespace Traps
{
    public class Spike : CellObjectBase, IInteractable
    {
        public void Interact()
        {
            EventManager.instance.onDeath?.Invoke(true);
            EventManager.instance.onPlaySfx?.Invoke(SoundsName.ActivateSpike);
        }

        public void StopInteract()
        {
        }

        public bool canPickUp
        {
            get => false;
            set { }
        }
    }
}
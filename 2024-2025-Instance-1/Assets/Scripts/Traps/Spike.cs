using Grid;

namespace Traps
{
    public class Spike : CellObjectBase, IInteractable
    {
        public void Interact()
        {
            EventManager.instance.onDeath?.Invoke(true);
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
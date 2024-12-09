using UnityEngine;
using Player;

namespace Grid
{
    public class WinCell : CellObjectBase, IInteractable
    {
        public bool CanPickUp {get; set;} = false;

        public void Interact(PlayerController controller)
        {
            Debug.Log("W");
        }

        public void StopInteract()
        {
        }
    }
}
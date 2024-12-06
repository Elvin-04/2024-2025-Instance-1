using UnityEngine;

namespace Grid
{
    public class WinCell : CellObjectBase, IInteractable
    {
        public bool CanPickUp {get; set;} = false;

        public void Interact()
        {
            Debug.Log("W");
        }

        public void StopInteract()
        {
        }
    }
}
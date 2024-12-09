using UnityEngine;
using Player;

namespace Grid
{
    public class WinCell : CellObjectBase, IInteractable
    {
        public bool CanPickUp {get; set;} = false;

        public void Interact()
        {
            EventManager.Instance.OnWin.Invoke();
        }

        public void StopInteract()
        {
        }
    }
}
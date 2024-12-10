namespace Grid
{
    public class WinCell : CellObjectBase, IInteractable
    {
        public bool canPickUp { get; set; } = false;

        public void Interact()
        {
            EventManager.instance.onWin.Invoke();
        }

        public void StopInteract()
        {
        }
    }
}
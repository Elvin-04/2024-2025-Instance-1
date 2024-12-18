namespace Grid
{
    public class WinCell : CellObjectBase, IInteractable
    {
        public bool canPickUp { get; set; } = false;

        public void Interact()
        {
            EventManager.instance.onWin.Invoke();
            EventManager.instance.onDisableInput.Invoke();
        }

        public void StopInteract()
        {
        }
    }
}
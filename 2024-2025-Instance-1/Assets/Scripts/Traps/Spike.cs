using Grid;
using Player;

public class Spike : CellObjectBase, IInteractable
{
    public void Interact()
    {
        EventManager.Instance.OnDeath?.Invoke();
    }

    public void StopInteract()
    {
    }

    public bool CanPickUp
    {
        get => false;
        set { }
    }
}
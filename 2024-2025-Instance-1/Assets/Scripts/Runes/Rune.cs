using Grid;
using UnityEngine;
using Player;

public abstract class Rune : CellObjectBase, IInteractableCallable, ICollisionObject
{
    protected bool _canPickUp = true;
    protected Color _color;
    public bool CanPickUp
    {
        get => _canPickUp;
        set => _canPickUp = value;
    }

    public virtual void ApplyEffect() { }
    public void Interact(PlayerController controller)
    {
        EventManager.Instance.AddRuneToInventory.Invoke(this);
    }

    public void StopInteract()
    {
       
    }
}
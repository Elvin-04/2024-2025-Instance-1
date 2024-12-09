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

    public abstract void ApplyEffect(Vector3 position, GridManager gridManager);
    public void Interact()
    {
        EventManager.Instance.AddRuneToInventory.Invoke(this);
    }

    public void StopInteract()
    {
       
    }
}
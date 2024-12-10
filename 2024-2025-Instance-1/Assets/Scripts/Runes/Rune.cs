using System;
using Grid;
using UnityEngine;

public abstract class Rune : CellObjectBase, IInteractableCallable, ICollisionObject
{
    protected bool _canPickUp = true;
    protected Color _color;
    
    public Action onDrop;
    public Action onTake;
    
    public bool CanPickUp
    {
        get => _canPickUp;
        set => _canPickUp = value;
    }

    public abstract void ApplyEffect(Vector3 position, GridManager gridManager);
    public void Interact()
    {
        onTake?.Invoke();
        EventManager.Instance.AddRuneToInventory.Invoke(this);
    }

    public void DropRune()
    {
        onDrop?.Invoke();
    }

    public void StopInteract()
    {
       
    }
}
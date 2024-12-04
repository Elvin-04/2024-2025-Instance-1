using UnityEngine;

public abstract class Rune : MonoBehaviour, IInteractable
{
    protected bool _canPickUp = true;
    protected Color _color;
    public bool CanPickUp 
    { 
        get => _canPickUp; 
        set => _canPickUp = value; 
    }

    public virtual void ApplyEffect() {}
    public void Interact() 
    {
        EventManager.Instance.AddRuneToInventory.Invoke(this);
    }
}

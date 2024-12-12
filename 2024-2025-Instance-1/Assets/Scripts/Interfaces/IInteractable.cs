public interface IInteractable
{
    bool canPickUp { get; set; }
    void Interact();
    void StopInteract();
}

public interface IInteractableCallable : IInteractable
{
}
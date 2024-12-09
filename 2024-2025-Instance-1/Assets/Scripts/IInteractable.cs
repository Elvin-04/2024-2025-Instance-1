using Player;

public interface IInteractable
{
    bool CanPickUp { get; set; }
    void Interact(PlayerController controller);
    void StopInteract();
}

public interface IInteractableCallable : IInteractable
{

}
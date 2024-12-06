using System;
using UnityEngine;

public interface IInteractable
{
    bool CanPickUp { get; set; }
    void Interact();
    void StopInteract();
}

public interface IInteractableCallable : IInteractable
{

}
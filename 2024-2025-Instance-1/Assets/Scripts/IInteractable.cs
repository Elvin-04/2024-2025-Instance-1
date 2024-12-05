using System;
using UnityEngine;

public interface IInteractable
{
    bool CanPickUp { get; set; }
    void Interact();
}

public interface IInteractableCallable : IInteractable
{

}
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(InventoryManager))]
public class TestPlayer : MonoBehaviour
{
    public GameObject go;
    public IInteractable interactable;
    private void Start()
    {
        EventManager.Instance.OnInteract.AddListener(Interact);
        interactable = go.GetComponent<Rune>();
    }

    private void Interact()
    {
        // Get Object to interact with who is on the case and call is interact In
        interactable.Interact();
    }

   
}

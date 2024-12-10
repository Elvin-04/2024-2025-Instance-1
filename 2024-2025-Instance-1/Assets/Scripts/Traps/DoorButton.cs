using Grid;
using System.Collections.Generic;
using UnityEngine;
using Player;

public class DoorButton : CellObjectBase, IInteractable
{
    private List<Transform> _doorTransforms = new();
    public Cell doorOpen;
    public Cell doorClose;

    public Cell wallCloseToDoorOpened;

    public Cell wallCloseToDoorClosedLeft;
    public Cell wallCloseToDoorClosedRight;
    public bool CanPickUp { get => false; set{} }

    public void Interact()
    {
        OpenDoors();
    }

    public void SetDoorTransforms(List<Transform> transforms)
    {
        _doorTransforms = transforms;
    }

    public void StopInteract() 
    {
        CloseDoors();
    }

    private void OpenDoors()
    {
        if (_doorTransforms == null)
        {
            return;
        }
        
        foreach (Transform t in _doorTransforms)
        {
            Open(t);
        }
    }

    private void CloseDoors()
    {
        foreach (Transform t in _doorTransforms)
        {
            Close(t);
        }
    }
    private void Open(Transform transform)
    {
        EventManager.Instance.OnChangeCell?.Invoke(transform.position, doorOpen);
        EventManager.Instance.OnChangeCell?.Invoke(transform.position + Vector3.right, wallCloseToDoorOpened);
        EventManager.Instance.OnChangeCell?.Invoke(transform.position + Vector3.left, wallCloseToDoorOpened);
    }

    private void Close(Transform transform)
    {
        EventManager.Instance.OnChangeCell?.Invoke(transform.position, doorClose);
        EventManager.Instance.OnChangeCell?.Invoke(transform.position + Vector3.right, wallCloseToDoorClosedLeft);
        EventManager.Instance.OnChangeCell?.Invoke(transform.position + Vector3.left, wallCloseToDoorClosedRight);
    }

}

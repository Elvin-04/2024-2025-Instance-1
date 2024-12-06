using Grid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorButton : CellObjectBase, IInteractable
{
    public List<Transform> doorTransforms;
    public TileBase doorOpen;
    public TileBase doorClose;
    public bool CanPickUp { get => false; set{} }

    public void Interact()
    {
        OpenDoors();
    }

    public void StopInteract() 
    {
        CloseDoors();
    }

    private void OpenDoors()
    {
        for(int i = 0; i < doorTransforms.Count; i++)
        {
            Open(doorTransforms[i]);
        }
    }

    private void CloseDoors()
    {
        for (int i = 0; i < doorTransforms.Count; i++)
        {
            Close(doorTransforms[i]);
        }
    }
    private void Open(Transform transform)
    {
        _cell?.gridManager.tilemap.SetTile(_cell.gridManager.tilemap.WorldToCell(transform.position), doorOpen);
    }

    private void Close(Transform transform)
    {
        _cell?.gridManager.tilemap.SetTile(_cell.gridManager.tilemap.WorldToCell(transform.position), doorClose);
    }

}

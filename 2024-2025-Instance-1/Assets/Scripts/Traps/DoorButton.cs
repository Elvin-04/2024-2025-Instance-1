using Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DoorButton : CellObjectBase, IInteractable
{
    public Transform doorTransform;
    public TileBase doorOpen;
    public TileBase doorClose;
    public bool CanPickUp { get => false; set{} }

    public void Interact()
    {
        Open();
    }

    public void StopInteract() 
    {
        Close();
    }

    private void Open()
    {
        _cell.gridManager.tilemap.SetTile(_cell.gridManager.tilemap.WorldToCell(doorTransform.position), doorOpen);
    }

    private void Close()
    {
        _cell.gridManager.tilemap.SetTile(_cell.gridManager.tilemap.WorldToCell(doorTransform.position), doorClose);
    }

}

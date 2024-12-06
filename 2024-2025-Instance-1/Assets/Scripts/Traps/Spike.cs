using Clock;
using Grid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class Spike : CellObjectBase, IInteractable
{
    [SerializeField] private GridManager _gridManager;
    [SerializeField] private TileBase _Active;
    [SerializeField] private TileBase _Inactive;

    private bool _isActive = false;

    private void Start()
    {
        SetTile(_isActive);
        EventManager.Instance.OnClockUpdated.AddListener(UpdateSpike);
    }
    public void UpdateSpike()
    {
        Debug.Log("Update");
        _isActive = !_isActive;
        SetTile(_isActive);
    }

    private TileBase GetTile(bool isActive)
    {
        return isActive ? _Active : _Inactive;
    }

    private void SetTile(bool isActive)
    {
        _gridManager.tilemap.SetTile(_gridManager.tilemap.WorldToCell(transform.position) , GetTile(_isActive));
    }

    public void Interact()
    {
        Debug.Log("Spike :: " + _isActive);
        if (!_isActive)
            return;
        EventManager.Instance.OnDeath?.Invoke();
        
    }

    public void StopInteract()
    {
        
    }

    public bool IsActive => _isActive;

    public bool CanPickUp { get => false; set{} }

    public void SetGridManager(GridManager value) => _gridManager = value;
}

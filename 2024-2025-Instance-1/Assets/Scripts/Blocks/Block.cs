using DG.Tweening;
using Grid;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public class Block : CellObjectBase, ICollisionObject, IInteractableInFront, IWeight
{
    public bool canPickUp { get; set; } = true;

    [SerializeField] private GridManager _gridManager;

    private Vector2 _playerDirection;

    private List<IWeightInteractable> _interactablesUnder = new();
    private Vector3 _interactablesUnderPosition;

    private void Start()
    {
        transform.position = _gridManager.GetCellPos(transform.position);
        _gridManager.AddObjectOnCell(transform.position, this);

        EventManager.instance.onMoveStarted.AddListener(GetDirection);
    }

    private void GetDirection(Vector2 direction)
    {
        _playerDirection = direction;
    }

    public void Interact()
    {
        Vector2Int cellIndex = _gridManager.GetCellIndex(transform.position);
        Vector2Int nextIndex = _gridManager.GetNextIndex(cellIndex, _playerDirection);

        if (_gridManager.GetCellObjectsByType(nextIndex, out List<ICollisionObject> _) || _gridManager.GetCell(nextIndex) == null)
            return;

        if (canPickUp)
        {
            transform.DOMove(_gridManager.GetCellPos(nextIndex), _gridManager.GetGlobalMoveTime())
                .OnComplete(GetInteractableUnderMe);
            
            _gridManager.RemoveObjectOnCell(cellIndex, this);
            _gridManager.AddObjectOnCell(nextIndex, this);
        }
    }

    private void GetInteractableUnderMe()
    {
        List<IWeightInteractable> interacts = new();

        foreach (CellObjectBase objectOnCell in _gridManager.GetObjectsOnCell(transform.position))
        {
            if (objectOnCell is IWeightInteractable interactable)
            {
                interacts.Add(interactable);
            }
        }

        List<IWeightInteractable> commonInteracts = new();

        foreach (IWeightInteractable interactable in _interactablesUnder)
        {
            if (interacts.Contains(interactable))
            {
                commonInteracts.Add(interactable);
            }
        }
        
        

        foreach (IWeightInteractable interactable in _interactablesUnder)
        {
            if (!commonInteracts.Contains(interactable))
            {
                if (_gridManager.GetCellObjectsByType(_interactablesUnderPosition, out List<IWeight> _))
                {
                    return;
                }
                if (interactable is IInteractable interact)
                {
                    interact.StopInteract();
                }
                
                interactable.StopWeightInteract();
            }
        }

        _interactablesUnder = interacts;
        _interactablesUnderPosition = transform.position;

        foreach (IWeightInteractable interactable in _interactablesUnder.ToList())
            interactable?.WeightInteract();
    }

    public void StopInteract()
    {
    }
}
using Grid;
using System.Linq;
using UnityEngine;
using DG.Tweening;
using System.Collections.Generic;

public class Block : CellObjectBase, ICollisionObject, IInteractableInFront, IWeight
{
    public bool canPickUp {get; set;} = true;

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

        if (_gridManager.GetCellObjectsByType(nextIndex, out List<ICollisionObject> nextObjects))
            return;

        if (canPickUp)
        {
            transform.DOMove(_gridManager.GetCellPos(nextIndex), _gridManager.GetGlobalMoveTime())
            .OnComplete(GetInteractableUnderMe);

            _gridManager.AddObjectOnCell(nextIndex, this);
            _gridManager.RemoveObjectOnCell(cellIndex, this);
        }
    }

    private void GetInteractableUnderMe()
        {
            List<IWeightInteractable> interacts =
                _gridManager.GetObjectsOnCell(transform.position)
                    .Select(cellObject => cellObject as IWeightInteractable).Where(interactable => interactable != null)
                    .ToList();

            List<IWeightInteractable> commonInteracts = interacts.Intersect(_interactablesUnder).ToList();

            _interactablesUnder.Except(commonInteracts).ToList()
                .ForEach(interact =>
                {
                    if (_gridManager.GetCellObjectsByType(_interactablesUnderPosition, out List<IWeight> _)) return;
                    (interact as IInteractable)?.StopInteract();
                    interact.StopWeightInteract();
                });

            _interactablesUnder = interacts;
            _interactablesUnderPosition = transform.position;

            foreach (IWeightInteractable interactable in _interactablesUnder.ToList()) 
                interactable?.WeightInteract();
        }

    public void StopInteract()
    {
    }
}

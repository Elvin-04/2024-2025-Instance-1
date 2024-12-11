using Grid;
using UnityEngine;
using DG.Tweening; 

public class Block : CellObjectBase, ICollisionObject, IInteractableInFront
{
    public bool canPickUp {get; set;} = true;

    [SerializeField] private GridManager _gridManager;

    private Vector2 _playerDirection;

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
        if (canPickUp)
            transform.DOMove(_gridManager.GetCellPos(_gridManager.GetNextIndex(_gridManager.GetCellIndex(transform.position), _playerDirection)), _gridManager.GetGlobalMoveTime());

            // Debug.Log(_playerDirection);
    }

    public void StopInteract()
    {
    }
}

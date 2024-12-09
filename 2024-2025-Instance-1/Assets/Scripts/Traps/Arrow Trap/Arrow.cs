using System.Linq;
using DG.Tweening;
using Grid;
using UnityEngine;

public class Arrow : CellObjectBase, IInteractable, IMoving
{
    [HideInInspector] public GridManager gridManager;
    public Direction directionEnum;
    public Vector2 direction;
    private Direction _direction1;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;

        EventManager.Instance.UpdateClock.AddListener(UpdateClock);
    }

    public bool CanPickUp { get; set; }

    public void Interact()
    {
        EventManager.Instance.OnDeath?.Invoke();
    }

    public void StopInteract()
    {
    }

    Vector2 IMoving.direction => direction;

    public void SetDirection(Direction direction)
    {
        transform.rotation = Quaternion.Euler(0, 0, (int)direction * 90);
        directionEnum = direction;
    }

    public void UpdateClock()
    {
        Vector2Int cellIndex = gridManager.GetCellIndex(_transform.position);

        direction = directionEnum switch
        {
            Direction.North => Vector2.up,
            Direction.South => Vector2.down,
            Direction.East => Vector2.right,
            Direction.West => Vector2.left,
            _ => Vector3.zero
        };

        Vector2Int nextIndex = gridManager.GetNextIndex(cellIndex, direction);

        if (gridManager.GetObjectsOnCell(gridManager.GetCellPos(nextIndex)).OfType<ICollisionObject>().Any())
        {
            EventManager.Instance.UpdateClock.RemoveListener(UpdateClock);
            Destroy(gameObject);
            return;
        }

        _transform.DOMove(gridManager.GetCellPos(nextIndex), 0.2f).OnComplete(() =>
        {
            gridManager.RemoveObjectOnCell(cellIndex, this);
        });
        gridManager.AddObjectOnCell(nextIndex, this);
    }
}
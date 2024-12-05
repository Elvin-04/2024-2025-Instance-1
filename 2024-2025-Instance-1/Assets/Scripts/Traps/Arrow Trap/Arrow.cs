using DG.Tweening;
using Grid;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [HideInInspector] public Vector2 direction;
    [HideInInspector] public ArrowTrap trap;
    [HideInInspector] public GridManager gridManager;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;

        EventManager.Instance.UpdateClock.AddListener(UpdateClock);
    }

    public void UpdateClock()
    {
        Cell nextCell = gridManager.GetCell(_transform.position + (Vector3) direction);

        // Vector3 trapCellPos = gridManager.WorldToCell(trap.transform.position);


        if (nextCell.objectOnCell is Wall)
        {
            EventManager.Instance.UpdateClock.RemoveListener(UpdateClock);
            Destroy(gameObject);
            return;
        }

        _transform.DOMove(_transform.position + (Vector3) direction, 0.2f);
    }
}
using System.Linq;
using DG.Tweening;
using Grid;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    [HideInInspector] public Vector3 direction;
    [HideInInspector] public GridManager gridManager;

    private Transform _transform;

    private void Start()
    {
        _transform = transform;

        EventManager.Instance.UpdateClock.AddListener(UpdateClock);
    }

    public void UpdateClock()
    {
        Vector2Int cellIndex = gridManager.GetCellIndex(_transform.position);
        (int, int) nextIndex = (cellIndex.x + (int) direction.x, cellIndex.y + (int) direction.y);

        foreach (CellObjectBase cellObjectBase in gridManager.GetObjectsOnCell(gridManager.GetCellPos(nextIndex)))
        {
            if (cellObjectBase is Wall)
            {
                EventManager.Instance.UpdateClock.RemoveListener(UpdateClock);
                Destroy(gameObject);
                return;
            }
        }

        _transform.DOMove(_transform.position + direction, 0.2f);
    }
}
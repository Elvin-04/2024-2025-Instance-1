using DG.Tweening;
using Grid;
using TMPro;
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
        int yMoveDir = Mathf.CeilToInt(Mathf.Abs(direction.y)) * (int)Mathf.Sign(direction.y);
        int xMoveDir = Mathf.CeilToInt(Mathf.Abs(direction.x)) * (int)Mathf.Sign(direction.x);
        (int, int) nextIndex = (cellIndex.x + xMoveDir, cellIndex.y + yMoveDir);

        if (gridManager.GetInstantiatedObject(gridManager.GetCellPos(nextIndex)) is Wall)
        {
            EventManager.Instance.UpdateClock.RemoveListener(UpdateClock);
            Destroy(gameObject);
            return;
        }
        _transform.DOMove(_transform.position + direction, 0.2f);
    }
}
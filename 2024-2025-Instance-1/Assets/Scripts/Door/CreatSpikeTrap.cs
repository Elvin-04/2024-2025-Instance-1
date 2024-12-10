using Grid;
using UnityEngine;

public class CreatSpikeTrap : ObjectCreator
{
    [SerializeField] private Cell _Active;
    [SerializeField] private Cell _Inactive;

    private bool _isActive;

    private void Start()
    {
        CreateCell(_gridManager, _Inactive, transform);
        SetTile(_isActive);
        EventManager.Instance.OnClockUpdated.AddListener(UpdateSpike);
    }

    public void UpdateSpike()
    {
        _isActive = !_isActive;
        SetTile(_isActive);
    }

    private Cell GetTile(bool isActive)
    {
        return isActive ? _Active : _Inactive;
    }

    private void SetTile(bool isActive)
    {
        EventManager.Instance.OnChangeCell?.Invoke(transform.position, GetTile(isActive));
    }

    private void OnDrawGizmos()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawCube(transform.position, Vector3.one * 0.5f);
    }
}
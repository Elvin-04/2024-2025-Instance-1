using Grid;
using UnityEngine;
using UnityEngine.Tilemaps;

[RequireComponent(typeof(Spike))]
public class CreatSpikeTrap : ObjectCreator
{
    [SerializeField] private TileBase _spike;

    private void Start()
    {
        Cell cell = CreatCellObject(_gridManager, _spike, transform);
        cell.UpdateCellRef(gameObject);
    }
}

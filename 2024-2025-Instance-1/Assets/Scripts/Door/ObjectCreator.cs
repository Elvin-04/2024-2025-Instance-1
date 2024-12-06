using System.Collections.Generic;
using Grid;
using UnityEngine;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] protected GridManager _gridManager;

    protected Cell CreateCell(GridManager gridManager, Cell tile, Transform spawnTransform)
    {
        gridManager.ChangeCell(spawnTransform.position, tile);
        Cell cell = gridManager.GetCell(spawnTransform.position);
        return cell;
    }

    protected List<Cell> CreateCells(GridManager gridManager, Cell tile, List<Transform> transforms)
    {
        List<Cell> cells = new();
        for (int i = 0; i < transforms.Count; i++)
        {
            Cell cell = CreateCell(gridManager, tile, transforms[i]);
            if (cell != null)
                cells.Add(cell);
        }

        return cells;
    }
}
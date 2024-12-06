using Grid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] protected GridManager _gridManager;
    protected Cell CreatCellObject(GridManager gridManager, Cell tile, Transform transform)
    {
        gridManager.ChangeCell(transform.position, tile);
        Cell cell = _gridManager.GetCell(transform.position);
        cell?.UpdateCellRef(cell.gameObject);
        return cell;
    }

    protected List<Cell> CreatCellObjects(GridManager gridManager, Cell tile, List<Transform> transforms)
    {
        List<Cell> cells = new List<Cell>();
        for(int i = 0; i < transforms.Count; i++) 
        {
            Cell cell = CreatCellObject(gridManager, tile, transforms[i]);
            if(cell != null)
                cells.Add(cell);
        }
        return cells;
    }
}

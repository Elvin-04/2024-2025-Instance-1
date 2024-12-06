using Grid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ObjectCreator : MonoBehaviour
{
    [SerializeField] protected GridManager _gridManager;
    protected Cell CreatCellObject(GridManager gridManager, TileBase tile, Transform transform)
    {
        Tilemap tilemap = gridManager.tilemap;
        tilemap.SetTile(tilemap.WorldToCell(transform.position), tile);
        GameObject go = tilemap.GetInstantiatedObject(tilemap.WorldToCell(transform.position));
        Cell cell = gridManager.GetCell(transform.position);
        cell?.UpdateCellRef(go);
        return cell;
    }

    protected List<Cell> CreatCellObjects(GridManager gridManager, TileBase tile, List<Transform> transforms)
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

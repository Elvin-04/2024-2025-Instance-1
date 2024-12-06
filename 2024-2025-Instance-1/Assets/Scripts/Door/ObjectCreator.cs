using Grid;
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
        cell.UpdateCellRef(go);
        return cell;
    }
}

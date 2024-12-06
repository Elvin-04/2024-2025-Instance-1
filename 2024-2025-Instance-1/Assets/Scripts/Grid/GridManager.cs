using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Cell _groundCell;
        [field: SerializeField] public Tilemap tilemap { get; private set; }

        private void Start()
        {
            //Asserts
            Assert.IsNotNull(tilemap, "tilemap is null in GridManager");
            Assert.IsNotNull(_groundCell, "the ground cell prefab is null in GridManager");
            
            EventManager.Instance.OnChangeCell?.AddListener(ChangeCell);
            EventManager.Instance.OnResetCell?.AddListener(ResetCell);
        }

        private void ResetCell(Vector3 pos)
        {
            ChangeCell(pos, _groundCell);
        }

        private void ChangeCell(Vector3 position, Cell toCell)
        {
            SetTile(WorldToCell(position), toCell);
        }

        public Vector3Int WorldToCell(Vector3 position)
        {
            return tilemap.WorldToCell(position);
        }

        public Cell GetCell(Vector3 position)
        {
            return GetCell(tilemap.WorldToCell(position));
        }

        public void SetTile(Vector3Int position, TileBase tile)
        {
            tilemap.SetTile(position, tile);
        }

        public Vector3 GetTilePosition(Vector3 position)
        {
            return tilemap.GetCellCenterWorld(tilemap.WorldToCell(position));
        }

        public Cell GetCell(Vector3Int position)
        {
            if (!tilemap.HasTile(position))
                return null;

            TileBase tile = tilemap.GetTile(position);
            Cell cell = tile as Cell;
            return cell;
        }
    }
}
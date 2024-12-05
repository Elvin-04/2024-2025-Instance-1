using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Tilemap _tilemap;
        public Cell[,] Cells { get; private set; }

        private void Start()
        {
            Cells = new Cell[_tilemap.size.x, _tilemap.size.y];

            Debug.Log("size : " + _tilemap.size);

            for (int x = 0; x < _tilemap.size.x; x++)
            {
                for (int y = 0; y < _tilemap.size.y; y++)
                {
                    Vector3Int tilePosition = new(x, y, 0);
                }
            }
        }

        public Vector3Int WorldToCell(Vector3 position)
        {
            return _tilemap.WorldToCell(position);
        }

        public Cell GetCell(Vector3 position)
        {
            return GetCell(_tilemap.WorldToCell(position));
        }

        public void SetTile(Vector3Int position, TileBase tile)
        {
            _tilemap.SetTile(position, tile);
        }


        public Vector3 GetTilePosition(Vector3 position)
        {
            return _tilemap.GetCellCenterWorld(_tilemap.WorldToCell(position));
        }

        public Cell GetCell(Vector3Int position)
        {
            if (!_tilemap.HasTile(position))
                return null;

            TileBase tile = _tilemap.GetTile(position);
            Cell cell = tile as Cell;
            return cell;
        }
    }
}
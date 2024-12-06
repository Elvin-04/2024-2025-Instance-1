using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Cell _groundCell;
        [field: SerializeField] public Tilemap tilemap { get; private set; }
        private Dictionary<(int, int), (Cell, Vector3)> _cells = new();

        private void Start()
        {
            //Asserts
            Assert.IsNotNull(tilemap, "tilemap is null in GridManager");
            Assert.IsNotNull(_groundCell, "the ground cell prefab is null in GridManager");

            EventManager.Instance.OnChangeCell?.AddListener(ChangeCell);
            EventManager.Instance.OnResetCell?.AddListener(ResetCell);

            for (int x = 0; x < tilemap.size.x; x++)
            {
                for (int y = 0; y < tilemap.size.y; y++)
                {
                    int xPos = x - tilemap.size.x / 2;
                    int yPos = y - tilemap.size.y / 2;
                    Vector3Int pos = Vector3Int.zero;
                    pos.Set(xPos, yPos, 0);
                    Cell cell = tilemap.GetTile<Cell>(pos);

                    if (cell == null)
                    {
                        continue;
                    }

                    Vector3 cellPos = tilemap.GetCellCenterWorld(pos);
                    
                    _cells[(x, y)] = (cell, cellPos);
                }
            }
        }
        
        public Vector2Int GetCellIndex(Vector3 position)
        {
            for (int x = 0; x < tilemap.size.x; x++)
            {
                for (int y = 0; y < tilemap.size.y; y++)
                {
                    if (_cells.ContainsKey((x, y)) &&
                        tilemap.WorldToCell(_cells[(x, y)].Item2) == tilemap.WorldToCell(position))
                    {
                        return new Vector2Int(x, y);
                    }
                }
            }

            return Vector2Int.zero;
        }

        #region AddCell

        public void AddCell((int, int) indexes, Cell cell, Vector3 cellPos)
        {
            _cells[(indexes.Item1, indexes.Item2)] = (cell, cellPos);
        }
        
        //Overload
        public void AddCell(int x, int y, Cell cell, Vector3 cellPos)
        {
            AddCell((x, y), cell, cellPos);
        }
        
        //Overload
        public void AddCell(Vector2Int indexes, Cell cell, Vector3 cellPos)
        {
            AddCell((indexes.x, indexes.y), cell, cellPos);
        }
        
        //Overload
        public void AddCell(Vector3 position, Cell cell, Vector3 cellPos)
        {
            AddCell(GetCellIndex(position), cell, cellPos);
        }

        #endregion
        
        #region GetCell

        public Cell GetCell((int, int) indexes)
        {
            if(!_cells.TryGetValue(indexes, out (Cell, Vector3) cellInfo))
            {
                return null;
            }
            
            Cell cell = cellInfo.Item1;
            return cell;
        }

        //Overload
        public Cell GetCell(int x, int y)
        {
            return GetCell((x, y));
        }

        //Overload
        public Cell GetCell(Vector2Int indexes)
        {
            return GetCell(indexes.x, indexes.y);
        }

        //Overload
        public Cell GetCell(Vector3 position)
        {
            Vector2Int indexes = GetCellIndex(position);
            return GetCell(indexes);
        }

        #endregion
        
        #region GetCellPos

        public Vector3 GetCellPos((int, int) indexes)
        {
            if(!_cells.TryGetValue(indexes, out (Cell, Vector3) cellInfo))
            {
                return Vector3.zero;
            }
            Vector3 cellPos = cellInfo.Item2;
            return cellPos;
        }

        //Overload
        public Vector3 GetCellPos(Vector3 position)
        {
            return GetCellPos(GetCellIndex(position));
        }

        //Overload
        public Vector3 GetCellPos(int x, int y)
        {
            return GetCellPos((x, y));
        }

        //Overload
        public Vector3 GetCellPos(Vector2Int indexes)
        {
            return GetCellPos(indexes.x, indexes.y);
        }

        #endregion
        
        #region ChangeCell
        
        public void ChangeCell((int, int) indexes, Cell toCell)
        {
            tilemap.SetTile(tilemap.WorldToCell(_cells[indexes].Item2), toCell);
        }

        //Overload
        public void ChangeCell(int x, int y, Cell toCell)
        {
            ChangeCell((x, y), toCell);
        }

        //Overload
        public void ChangeCell(Vector2Int indexes, Cell toCell)
        {
            ChangeCell(indexes.x, indexes.y, toCell);
        }

        //Overload
        public void ChangeCell(Vector3 position, Cell toCell)
        {
            Debug.Log(GetCellIndex(position));
            ChangeCell(GetCellIndex(position), toCell);
        }

        #endregion
        
        #region ResetCell

        public void ResetCell((int, int) indexes)
        {
            ChangeCell(indexes, _groundCell);
        }

        //Overload
        public void ResetCell(int x, int y)
        {
            ResetCell((x, y));
        }

        //Overload
        public void ResetCell(Vector2Int indexes)
        {
            ResetCell(indexes.x, indexes.y);
        }

        //Overload
        private void ResetCell(Vector3 position)
        {
            ResetCell(GetCellIndex(position));
        }
        
        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Cell _groundCell;
        [field: SerializeField] public Tilemap tilemap { get; private set; }
        private readonly Dictionary<(int, int), CellContainer> _cellsContainers = new();

        private void Awake()
        {
            //Asserts
            Assert.IsNotNull(tilemap, "tilemap is null in GridManager");
            Assert.IsNotNull(_groundCell, "the ground cell prefab is null in GridManager");

            //Create a copy of the tilemap DONT REMOVE !!!
            tilemap.gameObject.SetActive(false);
            tilemap = Instantiate(tilemap, tilemap.transform.parent);
            tilemap.gameObject.SetActive(true);

            for (int x = 0; x < tilemap.size.x; x++)
            for (int y = 0; y < tilemap.size.y; y++)
            {
                int xPos = x - tilemap.size.x / 2;
                int yPos = y - tilemap.size.y / 2;
                Vector3Int pos = Vector3Int.zero;
                pos.Set(xPos, yPos, 0);
                Cell cell = tilemap.GetTile<Cell>(pos);

                if (cell == null) continue;

                Vector3 cellPos = tilemap.GetCellCenterWorld(pos);
                //CreateCellAt(cellPos).name = "x : " + x + " y : " + y;
                _cellsContainers[(x, y)] = new CellContainer(cell, cellPos);
                _cellsContainers[(x, y)].AddObject(GetInstantiatedObject(cellPos));
            }
        }


        private void Start()
        {
            EventManager.Instance.OnChangeCell?.AddListener(ChangeCell);
            EventManager.Instance.OnResetCell?.AddListener(ResetCell);
        }

        private GameObject CreateCellAt(Vector3 pos)
        {
            GameObject cell = new()
            {
                transform =
                {
                    position = pos
                }
            };
            return cell;
        }

        private CellObjectBase GetInstantiatedObject(Vector3 pos)
        {
            return tilemap.GetInstantiatedObject(tilemap.WorldToCell(pos))?.GetComponent<CellObjectBase>();
        }

        public Vector2Int GetCellIndex(Vector3 position)
        {
            for (int x = 0; x < tilemap.size.x; x++)
            for (int y = 0; y < tilemap.size.y; y++)
                if (_cellsContainers.ContainsKey((x, y)) &&
                    tilemap.WorldToCell(_cellsContainers[(x, y)].cellPos) == tilemap.WorldToCell(position))
                    return new Vector2Int(x, y);

            return Vector2Int.zero;
        }

        #region RemoveObjectOnCell

        public void RemoveObjectOnCell((int, int) indexes, CellObjectBase cellObject)
        {
            _cellsContainers[indexes].RemoveObject(cellObject);
        }

        //Overload
        public void RemoveObjectOnCell(int x, int y, CellObjectBase cellObject)
        {
            RemoveObjectOnCell((x, y), cellObject);
        }

        //Overload
        public void RemoveObjectOnCell(Vector2Int indexes, CellObjectBase cellObject)
        {
            RemoveObjectOnCell((indexes.x, indexes.y), cellObject);
        }

        //Overload
        public void RemoveObjectOnCell(Vector3 pos, CellObjectBase cellObject)
        {
            RemoveObjectOnCell(GetCellIndex(pos), cellObject);
        }

        #endregion

        #region AddObjectOnCell

        public void AddObjectOnCell((int, int) indexes, CellObjectBase cellObject)
        {
            _cellsContainers[indexes].AddObject(cellObject);
        }

        public void AddObjectOnCell(int x, int y, CellObjectBase cellObject)
        {
            AddObjectOnCell((x, y), cellObject);
        }

        public void AddObjectOnCell(Vector2Int indexes, CellObjectBase cellObject)
        {
            AddObjectOnCell((indexes.x, indexes.y), cellObject);
        }

        public void AddObjectOnCell(Vector3 pos, CellObjectBase cellObject)
        {
            AddObjectOnCell(GetCellIndex(pos), cellObject);
        }

        #endregion

        #region GetObjectsOnCell

        public List<CellObjectBase> GetObjectsOnCell((int, int) indexes)
        {
            return _cellsContainers[indexes].objectsOnCell;
        }

        public List<CellObjectBase> GetObjectsOnCell(int x, int y)
        {
            return GetObjectsOnCell((x, y));
        }

        public List<CellObjectBase> GetObjectsOnCell(Vector2Int indexes)
        {
            return _cellsContainers[(indexes.x, indexes.y)].objectsOnCell;
        }

        public List<CellObjectBase> GetObjectsOnCell(Vector3 position)
        {
            Vector2Int indexes = GetCellIndex(position);
            return _cellsContainers[(indexes.x, indexes.y)].objectsOnCell;
        }

        #endregion

        #region IsType

        public bool GetCellObjectsByType<T>((int, int) indexes, out List<T> cellObjects)
        {
            cellObjects = _cellsContainers[indexes].objectsOnCell.OfType<T>().ToList();
            return cellObjects.Any();
        }

        public bool GetCellObjectsByType<T>(int x, int y, out List<T> cellObjects)
        {
            return GetCellObjectsByType((x, y), out cellObjects);
        }

        public bool GetCellObjectsByType<T>(Vector2Int indexes, out List<T> cellObjects)
        {
            return GetCellObjectsByType((indexes.x, indexes.y), out cellObjects);
        }

        public bool GetCellObjectsByType<T>(Vector3 pos, out List<T> cellObjects)
        {
            return GetCellObjectsByType(GetCellIndex(pos), out cellObjects);
        }

        #endregion

        #region GetCell

        public Cell GetCell((int, int) indexes)
        {
            if (!_cellsContainers.TryGetValue(indexes, out CellContainer cellInfo)) return null;

            Cell cell = cellInfo.cell;
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
            if (!_cellsContainers.TryGetValue(indexes, out CellContainer cellInfo)) return Vector3.zero;
            Vector3 cellPos = cellInfo.cellPos;
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
            Vector3Int pos = tilemap.WorldToCell(_cellsContainers[indexes].cellPos);
            tilemap.SetTile(pos, toCell);
            _cellsContainers[indexes] = new CellContainer(toCell, _cellsContainers[indexes].cellPos);
            _cellsContainers[indexes].AddObject(GetInstantiatedObject(pos));
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

        internal Vector3 GetTileSize()
        {
            return tilemap.cellSize;
        }


        #endregion
    }
}
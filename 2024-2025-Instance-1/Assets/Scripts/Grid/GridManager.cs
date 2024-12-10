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
        private readonly Dictionary<(int, int), CellContainer> _cellContainers = new();

        private void Awake()
        {
            //Asserts
            Assert.IsNotNull(tilemap, "tilemap is null in GridManager");
            Assert.IsNotNull(_groundCell, "the ground cell prefab is null in GridManager");

            //Create a copy of the tilemap DONT REMOVE !!!
            tilemap.gameObject.SetActive(false);
            tilemap = Instantiate(tilemap, tilemap.transform.parent);
            tilemap.gameObject.SetActive(true);

            int indexX = 0;
            int indexY = 0;

            for (int x = tilemap.origin.x; x < tilemap.origin.x + tilemap.size.x; x++)
            {
                for (int y = tilemap.origin.y; y < tilemap.origin.y + tilemap.size.y; y++)
                {
                    Vector3Int pos = Vector3Int.zero;
                    pos.Set(x, y, 0);
                    Cell cell = tilemap.GetTile<Cell>(pos);

                    if (cell == null)
                    {
                        indexY++;
                        continue;
                    }

                    Vector3 cellPos = tilemap.GetCellCenterWorld(pos);

                    //Debug
                    //CreateCellAt(cellPos).name = "x : " + indexX + " y : " + indexY;

                    _cellContainers[(indexX, indexY)] = new CellContainer(cell, cellPos);
                    if (cell.getPrefab)
                    {
                        GameObject goInstance = Instantiate(cell.getPrefab, pos, Quaternion.identity,
                            tilemap.transform);
                        cell.SetInstancedObject(goInstance);
                        _cellContainers[(indexX, indexY)].AddObject(goInstance.GetComponent<CellObjectBase>());
                    }

                    indexY++;
                }

                indexX++;
                indexY = 0;
            }
        }


        private void Start()
        {
            EventManager.instance.onChangeCell?.AddListener(ChangeCell);
            EventManager.instance.onResetCell?.AddListener(ResetCell);
            EventManager.instance.onRemoveObjectOnCell.AddListener(OnRemoveObjectOnCell);
            EventManager.instance.stopInteract.AddListener(OnStopInteract);
        }

        private void OnRemoveObjectOnCell(Vector3 pos, CellObjectBase cellObject)
        {
            RemoveObjectOnCell(pos, cellObject);
        }

        private void OnStopInteract(Vector3 pos)
        {
            List<CellObjectBase> objectsOnCell = GetObjectsOnCell(pos);
            if (!objectsOnCell.OfType<IWeight>().Any())
                objectsOnCell.OfType<IInteractable>().ToList()
                    .ForEach(objectOnCell => objectOnCell.StopInteract());
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

        public Vector2Int GetCellIndex(Vector3 position)
        {
            for (int x = 0; x < tilemap.size.x; x++)
            for (int y = 0; y < tilemap.size.y; y++)
                if (_cellContainers.ContainsKey((x, y)) &&
                    tilemap.WorldToCell(_cellContainers[(x, y)].cellPos) == tilemap.WorldToCell(position))
                    return new Vector2Int(x, y);

            return Vector2Int.zero;
        }

        #region GetNextIndex

        public Vector2Int GetNextIndex((int, int) indexes, Vector2 direction)
        {
            int yMoveDir = Mathf.CeilToInt(Mathf.Abs(direction.y)) * (int)Mathf.Sign(direction.y);
            int xMoveDir = Mathf.CeilToInt(Mathf.Abs(direction.x)) * (int)Mathf.Sign(direction.x);

            (int, int) nextIndex = (indexes.Item1 + xMoveDir, indexes.Item2 + yMoveDir);
            Vector2Int nextIndexVector = new(nextIndex.Item1, nextIndex.Item2);
            return nextIndexVector;
        }

        public Vector2Int GetNextIndex(int x, int y, Vector2 direction)
        {
            return GetNextIndex((x, y), direction);
        }

        public Vector2Int GetNextIndex(Vector2Int indexes, Vector2 direction)
        {
            return GetNextIndex((indexes.x, indexes.y), direction);
        }

        public Vector2Int GetNextIndex(Vector3 pos, Vector2 direction)
        {
            return GetNextIndex(GetCellIndex(pos), direction);
        }

        #endregion

        #region RemoveObjectOnCell

        public void RemoveObjectOnCell((int, int) indexes, CellObjectBase cellObject)
        {
            _cellContainers[indexes].RemoveObject(cellObject);
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
            _cellContainers[indexes].AddObject(cellObject);
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
            return _cellContainers[indexes].objectsOnCell;
        }

        public List<CellObjectBase> GetObjectsOnCell(int x, int y)
        {
            return GetObjectsOnCell((x, y));
        }

        public List<CellObjectBase> GetObjectsOnCell(Vector2Int indexes)
        {
            return _cellContainers[(indexes.x, indexes.y)].objectsOnCell;
        }

        public List<CellObjectBase> GetObjectsOnCell(Vector3 position)
        {
            Vector2Int indexes = GetCellIndex(position);
            return _cellContainers[(indexes.x, indexes.y)].objectsOnCell;
        }

        #endregion

        #region IsType

        public bool GetCellObjectsByType<T>((int, int) indexes, out List<T> cellObjects)
        {
            cellObjects = null;
            if (!_cellContainers.TryGetValue(indexes, out CellContainer cellInfo)) return false;
            cellObjects = _cellContainers[indexes].objectsOnCell.OfType<T>().ToList();
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
            if (!_cellContainers.TryGetValue(indexes, out CellContainer cellInfo)) return null;

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
            if (!_cellContainers.TryGetValue(indexes, out CellContainer cellInfo)) return Vector3.zero;
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
            Vector3Int pos = tilemap.WorldToCell(_cellContainers[indexes].cellPos);
            tilemap.SetTile(pos, toCell);
            Destroy(_cellContainers[indexes].cell.instancedObject);
            _cellContainers[indexes] = new CellContainer(toCell, _cellContainers[indexes].cellPos);
            if (toCell.getPrefab == null) return;

            GameObject goInstance = Instantiate(toCell.getPrefab, pos, Quaternion.identity, tilemap.transform);
            _cellContainers[indexes].AddObject(goInstance.GetComponent<CellObjectBase>());
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
            Debug.Log(indexes);
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
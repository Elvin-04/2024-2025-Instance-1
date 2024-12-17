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
        [SerializeField] private float _globalMoveTime;
        private readonly Dictionary<(int, int), CellContainer> _cellContainers = new();
        private int _countX;
        private int _countY;

        public Action<(int, int), CellObjectBase> onGameObjectInstanced;

        public Vector2Int size => new(_countX, _countY);

        private void Awake()
        {
            //Asserts
            Assert.IsNotNull(tilemap, "tilemap is null in GridManager");
            Assert.IsNotNull(_groundCell, "the ground cell prefab is null in GridManager");

            //Create a copy of the tilemap DONT REMOVE !!!
            tilemap.gameObject.SetActive(false);
            tilemap = Instantiate(tilemap, tilemap.transform.parent);
            tilemap.gameObject.SetActive(true);
            for (var x = tilemap.origin.x; x < tilemap.origin.x + tilemap.size.x; x++)
            {
                _countY = 0;
                for (var y = tilemap.origin.y; y < tilemap.origin.y + tilemap.size.y; y++)
                {
                    var pos = Vector3Int.zero;
                    pos.Set(x, y, 0);
                    var cell = tilemap.GetTile<Cell>(pos);

                    if (cell == null)
                    {
                        _countY++;
                        continue;
                    }

                    var cellPos = tilemap.GetCellCenterWorld(pos);

                    //Debug
                    //CreateCellAt(cellPos).name = "x : " + indexX + " y : " + indexY;

                    _cellContainers[(_countX, _countY)] = new CellContainer(cell, cellPos);
                    if (cell.getPrefab)
                    {
                        var goInstance = Instantiate(cell.getPrefab, cellPos, Quaternion.identity,
                            tilemap.transform);
                        SetInstancedObject((_countX, _countY), goInstance.GetComponent<CellObjectBase>());
                        _cellContainers[(_countX, _countY)].AddObject(goInstance.GetComponent<CellObjectBase>());
                    }

                    _countY++;
                }

                _countX++;
            }
        }


        private void Start()
        {
            EventManager.instance.onChangeCell?.AddListener(ChangeCell);
            EventManager.instance.onResetCell?.AddListener(ResetCell);
            EventManager.instance.onRemoveObjectOnCell.AddListener(OnRemoveObjectOnCell);
            EventManager.instance.stopInteract.AddListener(OnStopInteract);
        }

        private void SetInstancedObject((int, int) pos, CellObjectBase instancedObjectToSet)
        {
            //Debug.Log("invoke");
            _cellContainers[(pos.Item1, pos.Item2)]
                .SetInstancedObject(instancedObjectToSet.GetComponent<CellObjectBase>());
            onGameObjectInstanced?.Invoke(pos, instancedObjectToSet);
        }

        public bool IsCellGround(Cell cell)
        {
            return cell == _groundCell;
        }

        public float GetGlobalMoveTime()
        {
            return _globalMoveTime;
        }

        private void OnRemoveObjectOnCell(Vector3 pos, CellObjectBase cellObject)
        {
            RemoveObjectOnCell(pos, cellObject);
        }

        private void OnStopInteract(Vector3 pos)
        {
            var objectsOnCell = GetObjectsOnCell(pos);
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
            for (var x = 0; x < tilemap.size.x; x++)
            for (var y = 0; y < tilemap.size.y; y++)
                if (_cellContainers.ContainsKey((x, y)) &&
                    tilemap.WorldToCell(_cellContainers[(x, y)].cellPos) == tilemap.WorldToCell(position))
                    return new Vector2Int(x, y);

            return Vector2Int.zero;
        }

        #region GetCellContainer

        public CellContainer GetCellContainer((int, int) indexes)
        {
            return _cellContainers[indexes];
        }

        #endregion

        #region GetNextIndex

        public Vector2Int GetNextIndex((int, int) indexes, Vector2 direction)
        {
            var xMoveDir = direction.x > 0 ? 1 : direction.x < 0 ? -1 : 0;
            var yMoveDir = direction.y > 0 ? 1 : direction.y < 0 ? -1 : 0;

            var nextIndex = (indexes.Item1 + xMoveDir, indexes.Item2 + yMoveDir);
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
            EventManager.instance.onCellChanged?.Invoke(indexes);
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
            if (!_cellContainers[indexes].Contains(cellObject))
            {
                _cellContainers[indexes].AddObject(cellObject);
                EventManager.instance.onCellChanged?.Invoke(indexes);
            }
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
            return _cellContainers.TryGetValue(indexes, out var container) ? container.objectsOnCell : null;
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
            var indexes = GetCellIndex(position);
            return _cellContainers[(indexes.x, indexes.y)].objectsOnCell;
        }

        #endregion

        #region IsType

        public bool GetCellObjectsByType<T>((int, int) indexes, out List<T> cellObjects)
        {
            cellObjects = null;
            if (!_cellContainers.TryGetValue(indexes, out var cellInfo)) return false;
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
            if (!_cellContainers.TryGetValue(indexes, out var cellInfo)) return null;

            var cell = cellInfo.cell;
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
            var indexes = GetCellIndex(position);
            return GetCell(indexes);
        }

        #endregion

        #region GetCellPos

        public Vector3 GetCellPos((int, int) indexes)
        {
            if (!_cellContainers.TryGetValue(indexes, out var cellInfo)) return Vector3.zero;
            var cellPos = cellInfo.cellPos;
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
            CellContainer newCellContainer = new(toCell, GetCellPos(indexes));
            var currentCellContainer = _cellContainers[indexes];

            var pos = tilemap.WorldToCell(currentCellContainer.cellPos);
            tilemap.SetTile(pos, toCell);

            if (currentCellContainer.instancedObject != null)
            {
                var instancedObjectRemoved =
                    currentCellContainer.RemoveObject(currentCellContainer.instancedObject);
                Destroy(instancedObjectRemoved.gameObject);
            }

            newCellContainer.AddObjects(currentCellContainer.objectsOnCell);

            if (toCell.getPrefab != null)
            {
                var instancedCell = Instantiate(toCell.getPrefab, GetCellPos(indexes), Quaternion.identity,
                    tilemap.transform);
                var instancedCellObject = instancedCell.GetComponent<CellObjectBase>();
                _cellContainers[indexes] = newCellContainer;
                SetInstancedObject(indexes, instancedCellObject);
                newCellContainer.AddObject(instancedCellObject);
            }
            else
            {
                _cellContainers[indexes] = newCellContainer;
            }

            _cellContainers[indexes] = newCellContainer;
            EventManager.instance.onCellChanged?.Invoke(indexes);
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

        #endregion
    }
}
using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Assertions;
using UnityEngine.Tilemaps;
using static UnityEngine.Rendering.VolumeComponent;

namespace Grid
{
    public class GridManager : MonoBehaviour
    {
        [SerializeField] private Cell _groundCell;
        [field: SerializeField] public Tilemap staticTilemap { get; private set; }
        private readonly Dictionary<(int, int), CellContainer> _cellsContainers = new();

        private void Awake()
        {
            //Asserts
            Assert.IsNotNull(staticTilemap, "tilemap is null in GridManager");
            Assert.IsNotNull(_groundCell, "the ground cell prefab is null in GridManager");

            //Create a copy of the tilemap DONT REMOVE !!!
            staticTilemap.gameObject.SetActive(false);
            staticTilemap = Instantiate(staticTilemap, staticTilemap.transform.parent);
            staticTilemap.gameObject.SetActive(true);

            int indexX = 0;
            int indexY = 0;

            for (int x = staticTilemap.origin.x; x < staticTilemap.origin.x + staticTilemap.size.x; x++)
            {
                for (int y = staticTilemap.origin.y; y < staticTilemap.origin.y + staticTilemap.size.y; y++)
                {
                    Vector3Int pos = Vector3Int.zero;
                    pos.Set(x, y, 0);
                    Cell cell = staticTilemap.GetTile<Cell>(pos);

                    if (cell == null)
                    {
                        indexY++;
                        continue;
                    }

                    Vector3 cellPos = staticTilemap.GetCellCenterWorld(pos);
                    //CreateCellAt(cellPos).name = "x : " + indexX + " y : " + indexY;
                    _cellsContainers[(indexX, indexY)] = new CellContainer(cell, cellPos);
                    if(cell.Getprefab)
                    {
                        GameObject goInstance = Instantiate<GameObject>(cell.Getprefab, pos, Quaternion.identity, staticTilemap.transform);
                        _cellsContainers[(indexX, indexY)].AddObject(goInstance.GetComponent<CellObjectBase>());
                    }
                    indexY++;
                }
                indexX++;
                indexY = 0;
            }
        }


        private void Start()
        {
            EventManager.Instance.OnChangeCell?.AddListener(ChangeCell);
            EventManager.Instance.OnResetCell?.AddListener(ResetCell);
            EventManager.Instance.OnRemoveObjectOnCell.AddListener(OnRemoveObjectOnCell);
            EventManager.Instance.StopInteract.AddListener(OnStopInteract);
            
        }

        private void OnRemoveObjectOnCell(Vector3 pos, CellObjectBase cellObject)
        {
            RemoveObjectOnCell(pos, cellObject);
        }

        private void OnStopInteract(Vector3 pos)
        {
            Debug.Log("stopping interaction");
            List<CellObjectBase> objectsOnCell = GetObjectsOnCell(pos);
            if (!objectsOnCell.OfType<IWeight>().Any())
            {
                objectsOnCell.OfType<IInteractable>().ToList()
                    .ForEach(objectOnCell => objectOnCell.StopInteract());
            }
            
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

             CellObjectBase instantiatedObject = staticTilemap.GetInstantiatedObject(staticTilemap.WorldToCell(pos))
                ?.GetComponent<CellObjectBase>();

            return instantiatedObject;
        }

        public Vector2Int GetCellIndex(Vector3 position)
        {
            for (int x = 0; x < staticTilemap.size.x; x++)
            for (int y = 0; y < staticTilemap.size.y; y++)
                if (_cellsContainers.ContainsKey((x, y)) &&
                    staticTilemap.WorldToCell(_cellsContainers[(x, y)].cellPos) == staticTilemap.WorldToCell(position))
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
            cellObjects = null;
            if (!_cellsContainers.TryGetValue(indexes, out CellContainer cellInfo)) return false;
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
            Vector3Int pos = staticTilemap.WorldToCell(_cellsContainers[indexes].cellPos);
            staticTilemap.SetTile(pos, toCell);
            _cellsContainers[indexes].objectsOnCell.Select(obj=>obj.gameObject).ToList().ForEach(Destroy);
            _cellsContainers[indexes] = new CellContainer(toCell, _cellsContainers[indexes].cellPos);
            if(toCell.Getprefab != null)
            {
                GameObject goInstance = Instantiate<GameObject>(toCell.Getprefab, pos, Quaternion.identity, staticTilemap.transform);
                _cellsContainers[indexes].AddObject(goInstance.GetComponent<CellObjectBase>());
            }
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
using Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Debugs
{
    [RequireComponent(typeof(GridManager))]
    public class DebugObjectOnCell : MonoBehaviour
    {
        private GridManager _gridManager;
        private Dictionary<(int, int), CellObjectDebugger> _debuggers = new();

        private void Awake()
        {
            _gridManager = GetComponent<GridManager>();
        }

        private void Start()
        {
            Invoke(nameof(LateStart), 0);
            EventManager.instance.onCellChanged.AddListener(OnCellChanged);
        }

        private void LateStart()
        {
            for (int x = 0; x < _gridManager.size.x; x++)
            for (int y = 0; y < _gridManager.size.y; y++)
            {
                _debuggers.Add((x, y), CreateDebugger((x, y)));
            }
        }

        private CellObjectDebugger CreateDebugger((int, int) indexes)
        {
            Cell cell = _gridManager.GetCell(indexes);
            if (cell == null)
            {
                return null;
            }

            GameObject debuggerGo = new()
            {
                transform =
                {
                    position = _gridManager.GetCellPos(indexes),
                },
                name = indexes.ToString()
            };
            CellObjectDebugger debugger = debuggerGo.AddComponent<CellObjectDebugger>();
            debugger.Setup(GetCellObjects(indexes), _gridManager.GetCellContainer(indexes).instancedObject?.gameObject);
            return debugger;
        }

        private List<CellObjectBase> GetCellObjects((int, int) indexes)
        {
            List<CellObjectBase> cellObjects = new();
            List<CellObjectBase> objectsOnCell = _gridManager.GetObjectsOnCell((indexes.Item1, indexes.Item2));
            if (objectsOnCell == null)
            {
                return cellObjects;
            }

            foreach (CellObjectBase objectOnCell in objectsOnCell)
            {
                cellObjects.Add(objectOnCell);
            }

            return cellObjects;
        }

        private void OnCellChanged((int, int) indexes)
        {
            if (!_debuggers.TryGetValue(indexes, out CellObjectDebugger debugger))
            {
                return;
            }

            if (debugger != null)
            {
                _debuggers[indexes].Setup(GetCellObjects(indexes), _gridManager.GetCellContainer(indexes).instancedObject?.gameObject);
            }
        }
    }
}
using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Debugs
{
    public class CellObjectDebugger : MonoBehaviour
    {
        [SerializeField] private List<CellObjectBase> _cellObjectNames = new();

        public void SetCellObjectNames(List<CellObjectBase> cellObject)
        {
            _cellObjectNames = cellObject;
        }
    }
}
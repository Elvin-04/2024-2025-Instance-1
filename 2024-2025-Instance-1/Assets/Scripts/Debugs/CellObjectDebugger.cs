using Grid;
using System.Collections.Generic;
using UnityEngine;

namespace Debugs
{
    public class CellObjectDebugger : MonoBehaviour
    {
        [SerializeField] private GameObject _instancedObject;
        [SerializeField] private List<CellObjectBase> _cellObjectNames = new();

        public void Setup(List<CellObjectBase> cellObject, GameObject instancedObject)
        {
            _cellObjectNames = cellObject;
            _instancedObject = instancedObject;
        }
    }
}
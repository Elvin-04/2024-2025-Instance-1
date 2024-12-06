using System;
using UnityEngine;

namespace Grid
{
    [Serializable]
    public abstract class CellObjectBase : MonoBehaviour, ICellObject
    {
        public virtual bool IsEqual(CellObjectBase other)
        {
            return this == other;
        }
        protected Cell _cell;

        //To be called when instantiated
        public void SetCell(Cell cell)
        {
            _cell = cell;
        }
    }

    
}
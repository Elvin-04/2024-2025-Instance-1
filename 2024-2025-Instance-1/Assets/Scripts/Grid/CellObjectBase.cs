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
    }

    
}
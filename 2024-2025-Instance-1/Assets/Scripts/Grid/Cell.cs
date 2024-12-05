using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Cell : Tile
    {
        [field:SerializeField] public CellObjectBase objectOnCell { get; private set; }
        public void SetObjectOnCell(CellObjectBase objectOnCell)
        {
            this.objectOnCell = objectOnCell;
        }
    }
}


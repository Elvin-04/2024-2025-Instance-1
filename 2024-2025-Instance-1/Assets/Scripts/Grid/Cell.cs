using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Cell : Tile
    {
        [field:SerializeField] public CellObjectBase ObjectOnCell { get; private set; }
    }
}


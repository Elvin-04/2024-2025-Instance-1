using System;
using UnityEditor;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Cell : Tile
    {
        [SerializeField] private CellObject _objectOnCell = new CellObject();

        public CellObject ObjectOnCell
        {
            get => _objectOnCell;
            set => _objectOnCell = value;
        }
    }
}


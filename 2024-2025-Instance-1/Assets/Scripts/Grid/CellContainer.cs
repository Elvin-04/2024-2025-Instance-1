using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class CellContainer
    {
        public CellContainer(Cell cell, Vector3 cellPos)
        {
            this.cell = cell;
            this.cellPos = cellPos;
        }

        public Cell cell { get; private set; }

        public Vector3 cellPos { get; private set; }

        public List<CellObjectBase> objectsOnCell { get; } = new();

        public void AddObject(CellObjectBase cellObjectBase)
        {
            objectsOnCell.Add(cellObjectBase);
        }

        public void AddObjects(List<CellObjectBase> cellObjectBase)
        {
            objectsOnCell.AddRange(cellObjectBase);
        }

        public void RemoveObject(CellObjectBase cellObjectBase)
        {
            objectsOnCell.Remove(cellObjectBase);
        }

        public void RemoveObjects(List<CellObjectBase> cellObjectBase)
        {
            foreach (CellObjectBase cellObject in cellObjectBase) objectsOnCell.Remove(cellObject);
        }
    }
}
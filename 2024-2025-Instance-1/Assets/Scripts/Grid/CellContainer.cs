using System.Collections.Generic;
using UnityEngine;

namespace Grid
{
    public class CellContainer
    {
        public Cell cell { get; private set; }
        public Vector3 cellPos { get; private set; }
        public List<CellObjectBase> objectsOnCell { get; } = new();

        public CellContainer(Cell cell, Vector3 cellPos)
        {
            this.cell = cell;
            this.cellPos = cellPos;
        }

        public void Take(CellContainer container)
        {
            objectsOnCell.AddRange(container.objectsOnCell);
        }

        
        public void AddObject(CellObjectBase cellObjectBase)
        {
            objectsOnCell.Add(cellObjectBase);
        }

        public void AddObjects(List<CellObjectBase> cellObjectBase)
        {
            objectsOnCell.AddRange(cellObjectBase);
        }

        public void RemoveObject(CellObjectBase cellObject)
        {
            if (objectsOnCell.Contains(cellObject)) objectsOnCell.Remove(cellObject);
        }

        public void RemoveObjects(List<CellObjectBase> cellObjects)
        {
            foreach (CellObjectBase cellObject in cellObjects) RemoveObject(cellObject);
        }

        public bool Contains(CellObjectBase cellObject)
        {
            return objectsOnCell.Contains(cellObject);
        }
    }
}
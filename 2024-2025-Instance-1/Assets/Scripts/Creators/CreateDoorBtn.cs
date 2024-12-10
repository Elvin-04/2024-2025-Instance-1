using System.Collections.Generic;
using Grid;
using UnityEngine;
using UnityEngine.Assertions;

namespace Creators
{
    public class CreateDoorBtn : CellCreator
    {
        [SerializeField] private Cell _door;
        [SerializeField] private List<Transform> _doorTransforms;
        [SerializeField] protected GridManager _gridManager;

        #region CreateCell

        private Cell CreateCell(Cell tile, Transform spawnTransform)
        {
            _gridManager.ChangeCell(spawnTransform.position, tile);
            Cell cell = _gridManager.GetCell(spawnTransform.position);
            return cell;
        }

        private List<Cell> CreateCells(Cell tile, List<Transform> spawnTransforms)
        {
            List<Cell> cells = new();
            foreach (Transform spawnTransform in spawnTransforms)
            {
                Cell cell = CreateCell(tile, spawnTransform);
                if (cell != null)
                    cells.Add(cell);
            }

            return cells;
        }

        #endregion

        protected override void LateStart()
        {
            base.LateStart();
            
            _gridManager.GetObjectsOnCell(cellCreatorTransform.position).ForEach(objectOnCell =>
                (objectOnCell as DoorButton)?.SetDoorTransforms(_doorTransforms));

            CreateCells(_door, _doorTransforms);
        }

        protected override void OnDrawGizmos()
        {
            Assert.IsNotNull(_doorTransforms, "door transforms is null in CreateDoorBtn");
            base.OnDrawGizmos();
            Gizmos.color = Color.blue;
            foreach (Transform t in _doorTransforms)
            {
                Gizmos.color = Color.white;
                Gizmos.DrawCube(t.position, Vector3.one);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(t.position, cellCreatorTransform.position);
            }
        }
    }
}
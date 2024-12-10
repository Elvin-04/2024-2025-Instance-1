using System.Collections.Generic;
using Grid;
using Traps;
using UnityEngine;
using UnityEngine.Assertions;

namespace Creators
{
    public class CreateDoorBtn : CellCreator
    {
        [SerializeField] private Cell _door;
        [SerializeField] private List<Transform> _doorTransforms;
        [SerializeField] protected GridManager _gridManager;

        protected override void Start()
        {
            Assert.IsNotNull(_door, "door is null in CreateDoorBtn");
            Assert.IsNotNull(_doorTransforms, "doors transform is null in CreateDoorBtn");
            Assert.IsNotNull(_gridManager, "grid manager is null in CreateDoorBtn");
            base.Start();
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

        protected override void LateStart()
        {
            base.LateStart();

            _gridManager.GetObjectsOnCell(cellCreatorTransform.position).ForEach(objectOnCell =>
                (objectOnCell as DoorButton)?.SetDoorTransforms(_doorTransforms));

            CreateCells(_door, _doorTransforms);
        }

        #region CreateCell

        private void CreateCell(Cell tile, Transform spawnTransform)
        {
            _gridManager.ChangeCell(spawnTransform.position, tile);
            _gridManager.GetCell(spawnTransform.position);
        }

        private void CreateCells(Cell tile, List<Transform> spawnTransforms)
        {
            foreach (Transform spawnTransform in spawnTransforms) CreateCell(tile, spawnTransform);
        }

        #endregion
    }
}
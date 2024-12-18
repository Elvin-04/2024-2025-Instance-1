using System;
using System.Collections.Generic;
using System.Linq;
using Grid;
using Traps;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Assertions;

namespace Creators
{
    public class CreateDoorBtn : CellCreator
    {
        [SerializeField] private Cell _door;
        [SerializeField] private Cell _pillar;
        [SerializeField] private List<Transform> _doorTransforms;
        [SerializeField] private List<PillarObject> _pillars;
        [SerializeField] protected GridManager _gridManager;

        protected override void Start()
        {
            Assert.IsNotNull(_door, "door is null in CreateDoorBtn");
            Assert.IsNotNull(_doorTransforms, "doors transform is null in CreateDoorBtn");
            Assert.IsNotNull(_pillars, "pillars transform is null in CreateDoorBtn");
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
            Gizmos.color = Color.yellow;
            foreach (PillarObject t in _pillars)
            {
                Gizmos.color = t.side == Side.Right ? Color.blue : Color.red;
                Gizmos.DrawSphere(t.transform.position, .25f);
                Gizmos.color = Color.yellow;
                Gizmos.DrawLine(t.transform.position, cellCreatorTransform.position);
            }
        }

        protected override void LateStart()
        {
            base.LateStart();

            _gridManager.GetObjectsOnCell(cellCreatorTransform.position).ForEach(objectOnCell =>
                (objectOnCell as DoorButton)?.SetDoorTransforms(_doorTransforms));

            _gridManager.GetObjectsOnCell(cellCreatorTransform.position).ForEach(objectOnCell =>
                (objectOnCell as DoorButton)?.SetPillars(_pillars));

            CreateCells(_door, _doorTransforms);
            CreateCells(_pillar, _pillars.Select(pillar => pillar.transform).ToList());  
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

    [Serializable]
    public struct PillarObject
    {
        public Transform transform;
        public Side side;
    }

    [Serializable]
    public enum Side
    {
        Left, Right
    }
}
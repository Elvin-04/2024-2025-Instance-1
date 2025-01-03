using Grid;
using System;
using System.Collections.Generic;
using Traps;
using UnityEngine;
using UnityEngine.Assertions;

namespace Creators
{
    [RequireComponent(typeof(PressurePlate))]
    public class CreatePressurePlate : CellCreator
    {
        [Header("Manager")]
        [SerializeField] protected GridManager _gridManager;

        [Header("Transforms")]
        [SerializeField] private List<Transform> _doorTransforms;
        [SerializeField] private List<PillarObject> _pillars;

        [Header("Plate")]
        private PressurePlate _plate;

        protected override void Start()
        {
            Assert.IsNotNull(_doorTransforms, "doors transform is null in CreateDoorBtn");
            Assert.IsNotNull(_pillars, "pillars transform is null in CreateDoorBtn");
            Assert.IsNotNull(_gridManager, "grid manager is null in CreateDoorBtn");
            _plate = GetComponent<PressurePlate>();
            base.Start();

            EventManager.instance.onPlayerFinishedMoving.AddListener(OnPlayerFinishedMoving);
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

            /*_gridManager.GetObjectsOnCell(cellCreatorTransform.position).ForEach(objectOnCell =>
                (objectOnCell as PressurePlate)?.SetDoorTransforms(_doorTransforms));

            _gridManager.GetObjectsOnCell(cellCreatorTransform.position).ForEach(objectOnCell =>
                (objectOnCell as PressurePlate)?.SetPillars(_pillars));

            CreateCells(_door, _doorTransforms);
            CreateCells(_pillar, _pillars.Select(pillar => pillar.transform).ToList()); */
        }

        protected override void SetTile(Cell cell)
        {
            base.SetTile(cell);
            Invoke(nameof(SetupObjectsOnCell), 0);
        }

        private void SetupObjectsOnCell()
        {
            _gridManager.AddObjectOnCell(transform.position, _plate);
            _plate.SetDoorTransforms(_doorTransforms);
            _plate.SetPillars(_pillars);
            _plate.onPlate += OnPlate;
            _plate.offPlate += OffPlate;
            CreateCells(_plate.GetDoorClose, _doorTransforms);

            List<Transform> rightPillars = new();
            List<Transform> leftPillars = new();

            foreach (PillarObject p in _pillars)
            {
                if (p.side == Side.Right)
                    rightPillars.Add(p.transform);
                else
                    leftPillars.Add(p.transform);
            }

            CreateCells(_plate.GetPillarLeftClose, leftPillars);
            CreateCells(_plate.GetPillarRightClose, rightPillars);
            //Invoke(nameof(Test), 0);
        }


        private void OnPlayerFinishedMoving(Vector3 position)
        {
            if (_gridManager.GetCell(_gridManager.GetCellIndex(position)) == _plate.GetDoorClose)
            {
                EventManager.instance.onDeath.Invoke(false);
            }
        }

        private void Test()
        {
            _plate.StopInteract();
        }

        private void OnPlate()
        {
            _gridManager.AddObjectOnCell(transform.position, _plate);
        }

        private void OffPlate()
        {
            _gridManager.AddObjectOnCell(transform.position, _plate);
        }

        #region CreateCell

        private void CreateCell(Cell tile, Transform spawnTransform)
        {
            _gridManager.ChangeCell(spawnTransform.position, tile);
        }

        private void CreateCells(Cell tile, List<Transform> spawnTransforms)
        {
            foreach (Transform spawnTransform in spawnTransforms)
            {
                CreateCell(tile, spawnTransform);
            }
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
        Left,
        Right
    }
}
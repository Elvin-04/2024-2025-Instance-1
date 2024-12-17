using System;
using System.Collections.Generic;
using DeathSystem;
using Grid;
using Traps;
using UnityEngine;
using UnityEngine.Assertions;

namespace Creators
{
    public class CreatePressurePlate : CellCreator
    {
        [SerializeField] private Cell _door;
        [SerializeField] private Cell _pillarRight;
        [SerializeField] private Cell _pillarLeft;
        [SerializeField] private List<Transform> _doorTransforms;
        [SerializeField] private List<PillarObject> _pillars;
        [SerializeField] protected GridManager _gridManager;
        [SerializeField] private PressurePlate _plateToSpawn;
        private PressurePlate _plateSpawned;

        protected override void Start()
        {
            Assert.IsNotNull(_door, "door is null in CreateDoorBtn");
            Assert.IsNotNull(_doorTransforms, "doors transform is null in CreateDoorBtn");
            Assert.IsNotNull(_pillars, "pillars transform is null in CreateDoorBtn");
            Assert.IsNotNull(_gridManager, "grid manager is null in CreateDoorBtn");
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
            _plateSpawned = Instantiate(_plateToSpawn, transform.position, Quaternion.identity);
            Invoke(nameof(SetupObjectsOnCell), 0);
        }

        private void SetupObjectsOnCell()
        {
            _gridManager.AddObjectOnCell(transform.position, _plateSpawned);
            _plateSpawned.SetDoorTransforms(_doorTransforms);
            _plateSpawned.SetPillars(_pillars);
            _plateSpawned.onPlate += OnPlate;
            _plateSpawned.offPlate += OffPlate;
            CreateCells(_door, _doorTransforms);

            List<Transform> rightPillars = new();
            List<Transform> leftPillars = new();

            foreach (PillarObject p in _pillars)
            {
                if (p.side == Side.Right)
                    rightPillars.Add(p.transform);
                else
                    leftPillars.Add(p.transform);
            }

            CreateCells(_pillarLeft, leftPillars);
            CreateCells(_pillarRight, rightPillars);
            //Invoke(nameof(Test), 0);
        }


        private void OnPlayerFinishedMoving(Vector3 position)
        {
            if (_gridManager.GetCell(_gridManager.GetCellIndex(position)) == _door)
            {
                EventManager.instance.onDeath.Invoke(false);
            }
        }

        private void Test()
        {
            _plateSpawned.StopInteract();
        }

        private void OnPlate()
        {
            _gridManager.AddObjectOnCell(transform.position, _plateSpawned);
        }

        private void OffPlate()
        {
            _gridManager.AddObjectOnCell(transform.position, _plateSpawned);
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
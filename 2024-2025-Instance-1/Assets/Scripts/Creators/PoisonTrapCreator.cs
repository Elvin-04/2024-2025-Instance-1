using Grid;
using System;
using System.Collections;
using System.Collections.Generic;
using Traps;
using UnityEngine;

namespace Creators
{
    [Serializable]
    public struct PoisonRadius
    {
        [Range(0, 5)] public int top;
        [Range(0, 5)] public int bottom;
        [Range(0, 5)] public int left;
        [Range(0, 5)] public int right;
    }

    public class PoisonTrapCreator : CellCreator
    {
        [SerializeField] private int _maxTickClock;
        [SerializeField] private GridManager _gridManager;

        [SerializeField] private PoisonRadius _radius;


        [SerializeField] private Cell _poisonCell;
        private readonly List<(int, int)> _poisonCells = new();
        private int _currentTickClock;

        private Vector2Int _mainTrapIndex;
        private bool _playerPoisoned;

        protected override void Start()
        {
            base.Start();
            EventManager.instance.onPlayerFinishedMoving?.AddListener(OnClockUpdate);
            EventManager.instance.onDeath?.AddListener(OnDeath);
            _currentTickClock = _maxTickClock;
        }

        public int GetCurrentTick()
        {
            return _currentTickClock;
        }

        public int GetMaxTick()
        {
            return _maxTickClock;
        }

        protected override void SetTile(Cell cell)
        {
            var positionIndexes = _gridManager.GetCellIndex(transform.position);
            _mainTrapIndex = positionIndexes;

            for (var x = positionIndexes.x - _radius.left; x <= positionIndexes.x + _radius.right; x++)
            for (var y = positionIndexes.y - _radius.bottom; y <= positionIndexes.y + _radius.top; y++)
            {
                (int, int) position = (x, y);

                Cell cellToSpawn;

                if (x == positionIndexes.x && y == positionIndexes.y)
                {
                    cellToSpawn = cell;
                }
                else
                {
                    if (!_gridManager.IsCellGround(_gridManager.GetCell(position)))
                        continue;

                    cellToSpawn = _poisonCell;
                    _poisonCells.Add(position);
                }

                StartCoroutine(SetupPoison(position));


                // TODO CHECK IF THE CURRENT CELL IS "EMPTY"
                // AND DO NOT SPAWN THE CELL IF IT IS NOT EMPTY                    
                EventManager.instance.onChangeCell?.Invoke(_gridManager.GetCellPos(position), cellToSpawn);
            }
        }

        private IEnumerator SetupPoison((int, int) position)
        {
            yield return new WaitForEndOfFrame();
            if (_gridManager.GetCellContainer(position).instancedObject == null)
                Debug.Log("no instanced object");
            else
                _gridManager.GetCellContainer(position).instancedObject.GetComponent<PoisonTrap>().creator = this;
        }

        private void OnClockUpdate(Vector3 _)
        {
            if (!_playerPoisoned)
                return;

            UpdateLifeTime();
            CheckPlayerDied();
        }

        public void WeightInteract(PoisonTrap trap)
        {
            var pos = _gridManager.GetCellIndex(trap.transform.position);

            if (pos == _mainTrapIndex)
                foreach (var position in _poisonCells)
                {
                    Vector2Int currentPos = new()
                    {
                        x = position.Item1,
                        y = position.Item2
                    };
                    if (currentPos != pos) _gridManager.ResetCell(position);
                }
            //_gridManager.ResetCell(pos);
        }

        public void StopWeightInteract(PoisonTrap trap)
        {
            if (trap == null) return;

            Debug.Log("stopping weight interact");

            var pos = _gridManager.GetCellIndex(trap.transform.position);

            if (pos == _mainTrapIndex)
            {
                foreach (var position in _poisonCells)
                {
                    _gridManager.ChangeCell(position, _poisonCell);
                    StartCoroutine(SetupPoison(position));
                }
            }
            else
            {
                _gridManager.ChangeCell(pos, _poisonCell);
                StartCoroutine(SetupPoison((pos.x, pos.y)));
            }
        }

        public void PoisonPlayer()
        {
            _playerPoisoned = true;
            Debug.Log($"player poisoned : {_playerPoisoned}");
            EventManager.instance.onPoisonedPlayer?.Invoke();
        }

        public bool GetPoisoned()
        {
            return _playerPoisoned;
        }

        private void UpdateLifeTime()
        {
            _currentTickClock--;
        }

        private void CheckPlayerDied()
        {
            if (_currentTickClock < 0) EventManager.instance.onDeath?.Invoke(true);
        }

        
        private void OnDeath(bool deathEffect)
        {
            _currentTickClock = _maxTickClock;
            _playerPoisoned = false;
        }
    }
}
using System;
using System.Collections;
using System.Collections.Generic;
using Grid;
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
        [SerializeField] private GridManager _gridManager;

        [SerializeField] private PoisonRadius _radius;


        [SerializeField] private Cell _poisonCell;

        private Vector2Int _mainTrapIndex;
        private List<(int, int)> _poisonCells = new();

        protected override void SetTile(Cell cell)
        {
            Vector2Int positionIndexes = _gridManager.GetCellIndex(transform.position);
            _mainTrapIndex = positionIndexes;

            for (int x = positionIndexes.x - _radius.left; x <= positionIndexes.x + _radius.right; x++)
            {
                for (int y = positionIndexes.y - _radius.bottom; y <= positionIndexes.y + _radius.top; y++)
                {
                    (int, int) position = (x, y);

                    Cell cellToSpawn;

                    if (x == positionIndexes.x && y == positionIndexes.y)
                        cellToSpawn = cell;
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
        }

        private IEnumerator SetupPoison((int, int) position)
        {
            yield return new WaitForEndOfFrame();
            if (_gridManager.GetCellContainer(position).instancedObject == null)
            {
                Debug.Log("no instanced object");
            }
            else
            {
                _gridManager.GetCellContainer(position).instancedObject.GetComponent<PoisonTrap>().creator = this;
            }
        }

        public void WeightInteract(PoisonTrap trap)
        {
            Vector2Int pos = _gridManager.GetCellIndex(trap.transform.position);

            if (pos == _mainTrapIndex)
            {
                foreach ((int, int) position in _poisonCells)
                {
                    Vector2Int currentPos = new()
                    {
                        x = position.Item1,
                        y = position.Item2
                    };
                    if (currentPos != pos)
                    {
                        _gridManager.ResetCell(position);
                    }
                }
            }
            else
            {
                //_gridManager.ResetCell(pos);
            }
        }

        public void StopWeightInteract(PoisonTrap trap)
        {
            if (trap == null)
            {
                return;
            }

            Debug.Log("stopping weight interact");

            Vector2Int pos = _gridManager.GetCellIndex(trap.transform.position);

            if (pos == _mainTrapIndex)
            {
                foreach ((int, int) position in _poisonCells)
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
    }
}
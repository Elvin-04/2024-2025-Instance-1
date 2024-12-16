using System;
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

                    cellToSpawn.onGameObjectInstanciated += (obj) => obj.GetComponent<PoisonTrap>().creator = this;



                    // TODO CHECK IF THE CURRENT CELL IS "EMPTY"
                    // AND DO NOT SPAWN THE CELL IF IT IS NOT EMPTY                    
                    EventManager.instance.onChangeCell?.Invoke(_gridManager.GetCellPos(position), cellToSpawn);
                }
            }
        }

        public void WeightInteract(PoisonTrap trap)
        {
            if (_gridManager.GetCellIndex(trap.transform.position) != _mainTrapIndex)
                return;
            
            foreach ((int, int) position in _poisonCells)
                _gridManager.ResetCell(position);
        }

        public void StopWeightInteract(PoisonTrap trap)
        {
            if (_gridManager.GetCellIndex(trap.transform.position) != _mainTrapIndex)
                return;
            
            foreach ((int, int) position in _poisonCells)
                _gridManager.ChangeCell(position, _poisonCell);
        }
    }
}

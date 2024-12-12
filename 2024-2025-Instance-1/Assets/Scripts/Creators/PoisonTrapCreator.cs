using System.Collections.Generic;
using Grid;
using Traps;
using UnityEngine;

namespace Creators
{
    public class PoisonTrapCreator : CellCreator
    {
        [SerializeField] private GridManager _gridManager;

        [SerializeField] private int _radius;



        [SerializeField] private Cell _poisonCell;
        
        private PoisonTrap _mainTrap;
        private List<(int, int)> _poisonCells;

        protected override void SetTile(Cell cell)
        {
            Vector2Int positionIndexes = _gridManager.GetCellIndex(transform.position);

            for (int x = positionIndexes.x - _radius; x <= positionIndexes.x + _radius; x++)
            {
                for (int y = positionIndexes.y - _radius; y <= positionIndexes.y + _radius; y++)
                {
                    (int, int) position = (x, y);

                    Cell cellToSpawn = ((x == positionIndexes.x) && (y == positionIndexes.y)) ? cell :_poisonCell;
                    PoisonTrap trap = cellToSpawn.instancedObject.GetComponent<PoisonTrap>();

                    if (cellToSpawn == cell)
                        _mainTrap = trap;
                    else
                        _poisonCells.Add(position);

                    trap.creator = this;

                    // TODO CHECK IF THE CURRENT CELL IS "EMPTY"
                    // AND DO NOT SPAWN THE CELL IF IT IS EMPTY                    
                    EventManager.instance.onChangeCell?.Invoke(_gridManager.GetCellPos(position), cellToSpawn);
                }
            }
        }

        public void WeightInteract(PoisonTrap trap)
        {
            if (trap != _mainTrap)
                return;
            
            foreach ((int, int) position in _poisonCells)
                _gridManager.ResetCell(position);
        }

        public void StopWeightInteract(PoisonTrap trap)
        {
            if (trap != _mainTrap)
                return;
            
            foreach ((int, int) position in _poisonCells)
                _gridManager.ChangeCell(position, _poisonCell);
        }
    }
}

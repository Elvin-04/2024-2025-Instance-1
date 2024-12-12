using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Creators
{
    public class PoisonTrapCreator : CellCreator
    {
        [SerializeField] private GridManager _gridManager;

        [SerializeField] private int _radius;



        [SerializeField] private Cell _poisonCell;
        private List<(int, int)> _poisonCells = new();

        protected override void SetTile(Cell cell)
        {
            
            Vector2Int positionIndexes = _gridManager.GetCellIndex(transform.position);

            for (int x = positionIndexes.x - _radius; x <= positionIndexes.x + _radius; x++)
            {
                for (int y = positionIndexes.y - _radius; y <= positionIndexes.y + _radius; y++)
                {
                    (int, int) position = (x, y);
                    Cell cellToSpawn;

                    if ((x == positionIndexes.x) && (y == positionIndexes.y))
                    {
                        cellToSpawn = cell;
                    }
                    else
                    {
                        cellToSpawn = _poisonCell;
                        _poisonCells.Add(position);
                    }

                    // TODO CHECK IF THE CURRENT CELL IS "EMPTY"
                    // AND DO NOT SPAWN THE CELL IF IT IS EMPTY                    
                    EventManager.instance.onChangeCell?.Invoke(_gridManager.GetCellPos(position), cellToSpawn);
                }
            }
        }
    }
}

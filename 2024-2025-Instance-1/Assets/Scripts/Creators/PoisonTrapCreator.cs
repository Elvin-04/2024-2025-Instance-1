using System;
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
        
        private Vector2Int _mainTrapIndex;
        private List<(int, int)> _poisonCells = new();

        protected override void SetTile(Cell cell)
        {
            StartCoroutine(TaMère());

            Vector2Int positionIndexes = _gridManager.GetCellIndex(transform.position);
            _mainTrapIndex = positionIndexes;

            for (int x = positionIndexes.x - _radius; x <= positionIndexes.x + _radius; x++)
            {
                for (int y = positionIndexes.y - _radius; y <= positionIndexes.y + _radius; y++)
                {
                    (int, int) position = (x, y);
                    Cell cellToSpawn;
                    
                    if (x == positionIndexes.x && y == positionIndexes.y)
                        cellToSpawn = cell;
                    else
                    {
                        cellToSpawn = _poisonCell;
                        _poisonCells.Add(position);
                    }


                    // TODO CHECK IF THE CURRENT CELL IS "EMPTY"
                    // AND DO NOT SPAWN THE CELL IF IT IS NOT EMPTY                    
                    EventManager.instance.onChangeCell?.Invoke(_gridManager.GetCellPos(position), cellToSpawn);


                    // PoisonTrap trap = cellToSpawn.instancedObject.GetComponent<PoisonTrap>();

                    // if (cellToSpawn == cell)
                    //     _mainTrap = trap;
                    // else
                    //     _poisonCells.Add(position);

                    // trap.creator = this;
                }
            }
        }

        private IEnumerator<WaitForEndOfFrame> TaMère()
        {
            bool AAAAAAAAAAAAAAAAAAA;

            while (true)
            {
                try
                {
                    foreach (var index in _poisonCells)
                        _gridManager.GetCell(index).instancedObject.GetComponent<PoisonTrap>().creator = this;
            
                    _gridManager.GetCell(_mainTrapIndex).instancedObject.GetComponent<PoisonTrap>().creator = this;
                    
                    AAAAAAAAAAAAAAAAAAA = true;
                }
                catch (Exception)
                {
                    Debug.Log("JEAN NEYMAR");
                    AAAAAAAAAAAAAAAAAAA = false;
                }

                if (AAAAAAAAAAAAAAAAAAA)
                    yield break;
                else
                    yield return new WaitForEndOfFrame();
            }
            
        }

        public void WeightInteract(PoisonTrap trap)
        {
            TaMère();
            if (_gridManager.GetCellIndex(trap.transform.position) != _mainTrapIndex)
                return;
            
            foreach ((int, int) position in _poisonCells)
                _gridManager.ResetCell(position);
        }

        public void StopWeightInteract(PoisonTrap trap)
        {
            TaMère();
            if (_gridManager.GetCellIndex(trap.transform.position) != _mainTrapIndex)
                return;
            
            foreach ((int, int) position in _poisonCells)
                _gridManager.ChangeCell(position, _poisonCell);
        }
    }
}

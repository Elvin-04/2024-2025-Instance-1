using Grid;
using UnityEngine;

namespace Creators
{
    public class SwitchableCellCreator : CellCreator
    {
        [SerializeField] protected Cell _inactive;
        [SerializeField] [Range(1, 10)] protected int _tick;
        protected int _numberOfTick;
        protected Cell activeCell => _cellToSpawn;

        protected Cell GetTileBasedOnState(bool isActive)
        {
            return isActive ? activeCell : _inactive;
        }
    }
}
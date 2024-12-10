using Grid;
using UnityEngine;

namespace Creators
{
    public class SwitchableCellCreator : CellCreator
    {
        protected Cell activeCell => _cellToSpawn;
        [SerializeField] protected Cell _inactive;

        protected Cell GetTileBasedOnState(bool isActive)
        {
            return isActive ? activeCell : _inactive;
        }
    }
}
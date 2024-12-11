using Grid;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

namespace Creators
{
    public class PoisonTrapCreator : CellCreator
    {
        [SerializeField] private GridManager _gridManager;

        [SerializeField] private int _radius;

        protected override void SetTile(Cell cell)
        {
            
            Vector2Int positionIndexes = _gridManager.GetCellIndex(transform.position);

            for (int x = positionIndexes.x - _radius; x <= positionIndexes.x + _radius; x++)
                for (int y = positionIndexes.y - _radius; y <= positionIndexes.y + _radius; y++)
                {
                  
                    EventManager.instance.onChangeCell?.Invoke(_gridManager.GetCellPos((x,y)), cell);
                }

        }
    }
}

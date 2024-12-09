using Grid;
using NUnit.Framework;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Tilemaps;

public class ExplosionRune : Rune
{
    [SerializeField] private int _radius;
    public ExplosionRune(int radius)
    {
        this._radius = radius;
    }

    public override void ApplyEffect(Vector3 position, GridManager gridManager)
    {
        Vector2Int positionIndexes = gridManager.GetCellIndex(position);

        for (int x = positionIndexes.x - _radius; x <= positionIndexes.x + _radius; x ++)
        {
            for (int y = positionIndexes.y  - _radius ; y <= positionIndexes.y  + _radius ; y ++) 
            {
                gridManager.GetObjectsOnCell(x, y).ForEach(Debug.Log);
                if (gridManager.GetCellObjectsByType(x, y, out List<IExplosable> cellObjects))
                {
                    Debug.Log(cellObjects);
                    for (int i = 0; i < cellObjects.Count; i++) {
                        cellObjects[i].Explose();
                    }
                }
            }
        }
    }
}

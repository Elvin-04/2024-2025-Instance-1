using System.Collections.Generic;
using Grid;
using UnityEngine;

namespace Runes
{
    public class ExplosionRune : Rune
    {
        [SerializeField] private int _radius;
        [SerializeField] private GameObject _explosion;

        public override void ApplyEffect(Vector3 position, GridManager gridManager)
        {
            Vector2Int positionIndexes = gridManager.GetCellIndex(position);
            Instantiate(_explosion, position, Quaternion.identity);

            for (int x = positionIndexes.x - _radius; x <= positionIndexes.x + _radius; x++)
            for (int y = positionIndexes.y - _radius; y <= positionIndexes.y + _radius; y++)
            {
                if (!gridManager.GetCellObjectsByType(x, y, out List<IExplosive> cellObjects)) continue;

                foreach (IExplosive explosive in cellObjects) explosive.Explode();
            }
        }
    }
}
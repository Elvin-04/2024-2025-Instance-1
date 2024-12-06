using Grid;
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
        int offsetX = (int)gridManager.GetTileSize().x;
        int offsetY = (int)gridManager.GetTileSize().y;

        Vector2Int positionIndexes = gridManager.GetCellIndex(position);

        print(position.x - _radius * offsetX);

        for (int x = positionIndexes.x - _radius * offsetX; x < positionIndexes.x + _radius * offsetX; x += offsetX)
        {
            for (int y = positionIndexes.y  - _radius * offsetY; y < positionIndexes.y  + _radius * offsetY; y += offsetY) 
            {
                print(positionIndexes);
                if (gridManager.GetCell(x, y).objectOnCell is IExplosable explosableCell)
                {
                    explosableCell.Explose();
                }
            }
        }
    }
}

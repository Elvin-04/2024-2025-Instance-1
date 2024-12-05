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
        print("Test 1");
        Vector2 checkPos = new();

        int offsetX = (int)gridManager.GetTileSize().x;
        int offsetY = (int)gridManager.GetTileSize().y;

        for (int x = (int)position.x  - _radius * offsetX; x < (int)position.x  + _radius * offsetY; x += offsetX)
        {
            print("Test 2");
            for (int y = (int)position.y  - _radius * offsetY; y < (int)position.y  + _radius * offsetY; y += offsetY) 
            {
                checkPos.Set(x * gridManager.GetTileSize().x, y * gridManager.GetTileSize().y);
                print(checkPos);
                if (gridManager.GetCell(checkPos).objectOnCell is IExplosable explosableCell)
                {
                    explosableCell.Explose(gridManager);
                }
            }
        }
    }
}

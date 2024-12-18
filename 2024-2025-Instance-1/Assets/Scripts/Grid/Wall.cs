namespace Grid
{
    public class Wall : CellObjectBase, ICollisionObject
    {
        private void Awake()
        {
            gameObject.name = "wall : " + WallExtensions.id++;
        }
    }
    
}
public static class WallExtensions
{
    public static int id = 0;
}
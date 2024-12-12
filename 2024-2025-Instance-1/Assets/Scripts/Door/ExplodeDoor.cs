using Grid;

namespace Door
{
    public class ExplodeDoor : CellObjectBase, IExplosive, ICollisionObject
    {
        public void Explode()
        {
            EventManager.instance.onResetCell?.Invoke(transform.position);
        }
    }
}
using Grid;
using Managers.Audio;

namespace Door
{
    public class ExplodeDoor : CellObjectBase, IExplosive, ICollisionObject
    {
        public void Explode()
        {
            EventManager.instance.onResetCell?.Invoke(transform.position);
            EventManager.instance.onPlaySfx?.Invoke(SoundsName.BreakDoor, null);
        }
    }
}
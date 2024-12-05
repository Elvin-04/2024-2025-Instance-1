using System.Numerics;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Wall : CellObjectBase, ICollisionObject, IExplosable
    {
        //////////////////////////////////////
        public void Explose(GridManager gridManager)
        {
            print("je vais bientot devenir completement fou help");
            gridManager.SetTile(gridManager.WorldToCell(transform.position), null);
        }
        //////////////////////////////////////
    }
}

using System.Numerics;
using UnityEngine;
using UnityEngine.Tilemaps;

namespace Grid
{
    public class Wall : CellObjectBase, ICollisionObject, IExplosable
    {
        //////////////////////////////////////
        public void Explose()
        {
            EventManager.Instance.OnResetCell?.Invoke(transform.position);
            Debug.Log(transform.position);
        }
        //////////////////////////////////////
    }
}
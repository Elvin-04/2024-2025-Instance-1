using Grid;
using UnityEngine;

public class ExplodeDoor : CellObjectBase, IExplosable, ICollisionObject
{
    public void Explose()
    {
        Debug.Log("Normalement je dois fonctionner");
        EventManager.Instance.OnResetCell?.Invoke(transform.position);
    }
}

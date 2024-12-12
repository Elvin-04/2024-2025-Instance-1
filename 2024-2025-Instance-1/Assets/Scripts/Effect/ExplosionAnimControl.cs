using System;
using UnityEngine;

public class ExplosionAnimControl : MonoBehaviour
{
    public Action action;
    private void Explode()
    {
        action.Invoke();
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}

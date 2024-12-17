using System;
using UnityEngine;

public class ExplosionAnimControl : MonoBehaviour
{
    public Action action;
    [SerializeField] private Transform _feedbackTransform; 

    public void SetSize(int radius)
    {
        _feedbackTransform.localScale = new Vector3(2 * (radius + 1) - 1, 2 * (radius + 1) - 1, 2 * (radius + 1) - 1)/transform.localScale.x;
    }
    private void Explode()
    {
        action.Invoke();
    }
    private void Destroy()
    {
        Destroy(gameObject);
    }
}

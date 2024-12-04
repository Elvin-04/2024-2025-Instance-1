using UnityEngine;

[RequireComponent(typeof (Rigidbody2D), typeof (PolygonCollider2D))]
public class Arrow : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }
}
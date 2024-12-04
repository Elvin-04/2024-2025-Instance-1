using System;
using UnityEngine;

public enum Direction
{
    North = 0,
    East  = 1,
    South = 2,
    West  = 3,
}

public class ArrowTrap : MonoBehaviour
{
    private Transform _transform;

    [SerializeField] private Direction _direction;
    [SerializeField] private GameObject _arrowPrefab;

    [SerializeField] [Range(0f, 10f)] private float _fireDelay = 2f;
    [SerializeField] [Range(0f, 10f)] private float _arrowSpeed = 3f;

    private void Start()
    {
        _transform = transform;

        StartCoroutine(Utils.InvokeAfter(ShootArrow, _fireDelay));
    }

    private void ShootArrow()
    {
        StartCoroutine(Utils.InvokeAfter(ShootArrow, _fireDelay));

        GameObject arrow = Instantiate(_arrowPrefab);
        arrow.SetActive(true);
        
        Transform arrowTransform = arrow.transform;

        arrowTransform.position = _transform.position;
        arrowTransform.rotation = Quaternion.Euler(0, 0, (int) _direction * 90);

        arrow.GetComponent<Rigidbody2D>().linearVelocity = arrowTransform.rotation * new Vector2(0, _arrowSpeed);
    }
}

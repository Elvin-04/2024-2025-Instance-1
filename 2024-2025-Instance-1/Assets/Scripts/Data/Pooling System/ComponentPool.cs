using UnityEngine;
using UnityEngine.Assertions;

public class ComponentPool<T> : IPool<T> where T : Component, IPooledObject<T>
{
    private readonly Pool<T> _pool;
    public int PooledObjectsCount => _pool.PooledObjectsCount;

    public int AliveobjectsCount => _pool.AliveobjectsCount;


    public ComponentPool(GameObject prefab, int capacity, int preAllocateCount = 0)
    {
        Assert.IsNotNull(prefab, "ComponentPool : prefab cannot be null");
        _pool = new(
            () =>
            {
                GameObject gameObject = Object.Instantiate(prefab);
                T component = gameObject.GetComponent<T>();
                return component;
            },

            (component) =>
            {
                component.gameObject.SetActive(true);
                component.GetComponent<Rigidbody>().linearVelocity = Vector3.zero;
            },

            (component) => { component.gameObject.SetActive(false); }
            
            , capacity, preAllocateCount);
    }

    public T Get()
    {
        return _pool.Get();
    }

    public void Release(T obj)
    {
        _pool.Release(obj);
    }
}
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.Assertions;

public class Pool<T> : IPool<T> where T : class, IPooledObject<T>
{
    private readonly Func<T> _createFunc;
    private readonly Action<T> _onGetFunc;
    private readonly Action<T> _onReleaseFunc;
    private readonly Stack<T> _pooledObjectsStack;
    private int _aliveObjectsCount;

    public int PooledObjectsCount => _pooledObjectsStack.Count;

    public int AliveobjectsCount => _aliveObjectsCount;



    public Pool (Func<T> createFunc, int capacity = 50, int preAllocateCount = 0) 
        : this(createFunc, null, null, capacity, preAllocateCount) {}

    public Pool(Func<T> createFunc, Action<T> onGetFunc, Action<T> onReleaseFunc,int capacity = 50, int preAllocateCount = 0)
    {
        Assert.IsNotNull(createFunc, "Pool: createFunc cannot be null");
        Assert.IsTrue(capacity >= 1, "Pool: capacity must be at least 1");
        Assert.IsTrue(preAllocateCount >= 0, "Pool: preAllocateCount must be at least 0");

        _pooledObjectsStack = new(preAllocateCount);
        _createFunc = createFunc;
        _onGetFunc = onGetFunc;
        _onReleaseFunc = onReleaseFunc;
        _aliveObjectsCount = 0;
        PreAllocatePooledObjects(preAllocateCount);
    }

    void PreAllocatePooledObjects(int preAllocateCount)
    {
        for (int i = 0; i < preAllocateCount; i++)
        {
            T pooledObject = CreatePoolObject();
            Release(pooledObject);
        }
    }

    private T CreatePoolObject()
    { 
        T pooledObject = _createFunc.Invoke();
        Assert.IsNotNull(pooledObject, "Pool: createFunc returned null");
        pooledObject.SetPool(this);
        return pooledObject;
    }

    public T Get()
    {
        T pooledObject;

        if (_pooledObjectsStack.Count > 0)
        {
            pooledObject = _pooledObjectsStack.Pop();
        }
        else
        {
            pooledObject = CreatePoolObject();
        }

        _aliveObjectsCount++;
        _onGetFunc?.Invoke(pooledObject);
        return pooledObject;
    }

    public void Release(T pooledObject)
    {
        Assert.IsNotNull(pooledObject, "Pool: pooledObject cannot be null");
        _pooledObjectsStack.Push(pooledObject); // mettre dans la liste .stack
        _aliveObjectsCount--;
        _onReleaseFunc?.Invoke(pooledObject);
    }
}
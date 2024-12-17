using System;

public interface IPooledObject<T> where T : class, IPooledObject<T>
{
    void SetPool(Pool<T> pool);
}
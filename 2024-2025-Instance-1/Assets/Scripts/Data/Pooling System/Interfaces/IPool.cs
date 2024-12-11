
public interface IPool <T>
{
    int PooledObjectsCount { get; }
    int AliveobjectsCount { get; }

    T Get();
    void Release(T obj);
}
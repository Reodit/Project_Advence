using UnityEngine;

public abstract class GenericObjectPooler<T> where T : Component, IPooledObject
{
    protected Transform parent;

    public GenericObjectPooler(Transform parent)
    {
        this.parent = parent;
    }

    public abstract T GetFromPool(T t);

    public abstract void ReturnToPool(T t);

    public abstract T GetFromPool(T t, Vector2 pos, Quaternion quat, Transform parent);
}

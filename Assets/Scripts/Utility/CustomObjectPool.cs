using System.Collections.Generic;
using UnityEngine;

public abstract class CustomObjectPool<T> where T : Component, IPooledObject
{
    protected int maxSize;
    protected int initSize;
    protected Queue<T> pool = new Queue<T>();

    protected T prefab;
    protected Transform parent;

    public CustomObjectPool(T t, Transform parent, int initialSize = 10, int maxSize = 100)
    {
        prefab = t;
        this.parent = parent;
        initSize = initialSize;
        this.maxSize = maxSize;
        
        for (int i = 0; i < initialSize; i++)
        {
            CreateNewInstance();
        }
    }

    protected virtual void CreateNewInstance()
    {
        T t = UnityEngine.Object.Instantiate(prefab, parent);
        t.gameObject.SetActive(false);
        pool.Enqueue(t);
    }

    protected void CreateNewInstances()
    {
        for(int i = 0; i < initSize; i++)
        {
            CreateNewInstance();
        }
    }

    public virtual T Get()
    {
        if (maxSize < pool.Count)
            return null;

        T t;
        if (pool.Count < initSize / 2)
            CreateNewInstances();

        t = pool.Dequeue();
        t.gameObject.SetActive(true);
        return t;
    }

    public virtual void Return(T t)
    {
        t.gameObject.SetActive(false);
        pool.Enqueue(t);
        t.OnObjectReturn();
    }
}

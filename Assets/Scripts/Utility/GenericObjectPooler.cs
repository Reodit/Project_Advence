using NPOI.POIFS.Properties;
using System.Collections;
using System.Collections.Generic;
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
}

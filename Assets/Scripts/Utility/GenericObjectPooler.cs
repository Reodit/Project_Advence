using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class GenericObjectPooler<T> where T : Component, IPooledObject
{
    public abstract T Instantiate(T t, Vector3 pos, Quaternion quat);

    public abstract void ReturnToPool(T t);
}

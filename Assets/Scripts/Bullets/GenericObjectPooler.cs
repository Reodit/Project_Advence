using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GenericObjectPooler<PoolObject, Pool> where PoolObject : Component, IPooledObject where Pool : GenericObjectPool<PoolObject>
{
    protected Dictionary<PoolObject, Pool> objectPoolDict = new Dictionary<PoolObject, Pool>();

    public PoolObject Instantiate(PoolObject poolObject, Vector3 pos, Quaternion quat)
    {
        if (poolObject == null)
            return null;

        if (!objectPoolDict.ContainsKey(poolObject))
        {
            Pool pool = Activator.CreateInstance(typeof(Pool), poolObject) as Pool;
            objectPoolDict.Add(poolObject, pool);
        }

        PoolObject newT1 = objectPoolDict[poolObject].Get();
        newT1.transform.SetPositionAndRotation(pos, quat);

        return newT1;
    }
}

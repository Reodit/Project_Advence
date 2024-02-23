using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : CustomObjectPool<Bullet>
{
    public BulletObjectPool(Bullet prefab, Transform parent, int initialSize = 10, int maxSize = 100) : base(prefab, parent, initialSize, maxSize)
    {
    }
}

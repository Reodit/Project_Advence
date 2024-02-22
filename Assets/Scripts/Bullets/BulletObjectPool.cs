using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPool : GenericObjectPool<Bullet>
{
    public BulletObjectPool(Bullet prefab, int initialSize = 10, int maxSize = 100) : base(prefab, initialSize, maxSize)
    {
    }
}

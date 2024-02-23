using MathNet.Numerics.Statistics;
using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPooler
{
    private Dictionary<int, BulletObjectPool> _poolDict = new Dictionary<int, BulletObjectPool>();
    protected Transform parent;

    public BulletObjectPooler(Transform parent)
    {
        this.parent = parent;
    }

    public Bullet GetFromPool(Bullet bulletPrefab)
    {
        int index = bulletPrefab.SkillIndex;
        if (!_poolDict.ContainsKey(bulletPrefab.SkillIndex))
            _poolDict.Add(index, new BulletObjectPool(bulletPrefab, parent));

        return _poolDict[index].Get();
    }

    public void ReturnToPool(Bullet bullet)
    {
        _poolDict[bullet.SkillIndex].Return(bullet);
    }
}

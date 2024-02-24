using MathNet.Numerics.Statistics;
using NPOI.POIFS.Properties;
using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPooler : GenericObjectPooler<Bullet>
{
    private Dictionary<int, BulletObjectPool> _poolDict = new Dictionary<int, BulletObjectPool>();

    public BulletObjectPooler(Transform parent) : base(parent)
    {
    }

    public override Bullet GetFromPool(Bullet bulletPrefab)
    {
        int index = bulletPrefab.SkillIndex;
        if (!_poolDict.ContainsKey(bulletPrefab.SkillIndex))
            _poolDict.Add(index, new BulletObjectPool(bulletPrefab, parent));

        return _poolDict[index].Get();
    }

    public override Bullet GetFromPool(Bullet bulletPrefab, Vector2 pos, Quaternion quat, Transform parent)
    {
        int index = bulletPrefab.SkillIndex;
        if (!_poolDict.ContainsKey(index))
            _poolDict.Add(index, new BulletObjectPool(bulletPrefab, parent));

        Bullet bullet = _poolDict[index].Get();
        bullet.transform.SetParent(parent);
        bullet.transform.SetPositionAndRotation(pos, quat);

        return bullet;
    }

    public override void ReturnToPool(Bullet bullet)
    {
        _poolDict[bullet.SkillIndex].Return(bullet);
    }
}

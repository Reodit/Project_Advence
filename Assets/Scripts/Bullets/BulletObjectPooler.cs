using MathNet.Numerics.Statistics;
using NPOI.SS.Formula.Functions;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BulletObjectPooler : GenericObjectPooler<Bullet>
{
    private Dictionary<int, BulletObjectPool> _poolDict = new Dictionary<int, BulletObjectPool>();

    public override Bullet Instantiate(Bullet bullet, Vector3 pos, Quaternion quat)
    {
        if (bullet == null)
            return null;

        int bulletIndex = bullet.SkillIndex;

        if (!_poolDict.ContainsKey(bulletIndex))
            _poolDict.Add(bulletIndex, new BulletObjectPool(bullet));

        Bullet newBullet = _poolDict[bulletIndex].Get();
        newBullet.transform.SetPositionAndRotation(pos, quat);

        return newBullet;
    }

    public override void ReturnToPool(Bullet bullet)
    {
        _poolDict[bullet.SkillIndex].Release(bullet);
    }
}

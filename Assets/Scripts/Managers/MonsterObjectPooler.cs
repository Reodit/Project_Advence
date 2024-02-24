using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPooler : GenericObjectPooler<Monster>
{
    private Dictionary<int, BulletObjectPool> _poolDict = new Dictionary<int, BulletObjectPool>();

    public MonsterObjectPooler(Transform parent) : base(parent)
    {
    }

    public override Monster GetFromPool(Monster t)
    {
        throw new System.NotImplementedException();
    }

    public override void ReturnToPool(Monster t)
    {
        throw new System.NotImplementedException();
    }

    public override Monster GetFromPool(Monster t, Vector2 pos, Quaternion quat, Transform parent)
    {
        throw new System.NotImplementedException();
    }
}
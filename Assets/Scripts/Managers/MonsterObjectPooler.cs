using MathNet.Numerics.Statistics;
using System.Collections.Generic;
using UnityEngine;

public class MonsterObjectPooler : GenericObjectPooler<Monster>
{
    private Dictionary<int, MonsterObjectPool> _poolDict = new Dictionary<int, MonsterObjectPool>();

    public MonsterObjectPooler(Transform parent) : base(parent)
    {
    }

    public override Monster GetFromPool(Monster monster)
    {
        int index = monster.DataKey;
        if (!_poolDict.ContainsKey(index))
            _poolDict.Add(index, new MonsterObjectPool(monster, parent));

        return _poolDict[index].Get();
    }

    public override void ReturnToPool(Monster monster)
    {
        _poolDict[monster.DataKey].Return(monster);
    }

    public override Monster GetFromPool(Monster monsterPrefab, Vector2 pos, Quaternion quat, Transform parent)
    {
        int index = monsterPrefab.DataKey;
        if (!_poolDict.ContainsKey(index))
            _poolDict.Add(index, new MonsterObjectPool(monsterPrefab, parent));

        Monster monster = _poolDict[index].Get();
        monster.transform.SetParent(parent);
        monster.transform.SetPositionAndRotation(pos, quat);

        return monster;
    }
}
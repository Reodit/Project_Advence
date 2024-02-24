using MathNet.Numerics.Statistics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObjectPooler : GenericObjectPooler<ParticlePoolObject>
{
    private Dictionary<string, ParticleObjectPool> _poolDict = new Dictionary<string, ParticleObjectPool>();

    public ParticleObjectPooler(Transform parent) : base(parent)
    {
    }

    public override ParticlePoolObject GetFromPool(ParticlePoolObject particlePrefab)
    {
        string prefabName = particlePrefab.name;
        if (!_poolDict.ContainsKey(prefabName))
            _poolDict.Add(prefabName, new ParticleObjectPool(particlePrefab, parent));

        return _poolDict[prefabName].Get();
    }

    public override ParticlePoolObject GetFromPool(ParticlePoolObject particlePrefab, Vector2 pos, Quaternion quat, Transform parent)
    {
        return null;
    }

    public override void ReturnToPool(ParticlePoolObject particle)
    {
        _poolDict[particle.name].Return(particle);
    }
}

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

    public ParticlePoolObject GetFromPool(ParticlePoolObject particlePrefab, Vector2 pos, Quaternion quat)
    {
        string prefabName = particlePrefab.name;
        if (!_poolDict.ContainsKey(prefabName))
            _poolDict.Add(prefabName, new ParticleObjectPool(particlePrefab, parent));

        ParticlePoolObject particle = _poolDict[prefabName].Get();
        particle.transform.SetPositionAndRotation(pos, quat);

        return particle;
    }

    public override ParticlePoolObject GetFromPool(ParticlePoolObject particlePrefab, Vector2 pos, Quaternion quat, Transform parent)
    {
        string prefabName = particlePrefab.name;
        if (!_poolDict.ContainsKey(prefabName))
            _poolDict.Add(prefabName, new ParticleObjectPool(particlePrefab, parent));

        ParticlePoolObject particle = _poolDict[prefabName].Get();
        particle.transform.SetParent(parent);
        particle.transform.SetPositionAndRotation(pos, quat);

        return particle;
    }

    public override void ReturnToPool(ParticlePoolObject particle)
    {
        _poolDict[particle.name].Return(particle);
    }
}
using System.Collections.Generic;
using UnityEngine;

public class ParticleSystemPooler
{
    private Dictionary<string, ParticleObjectPool> _poolDict = new Dictionary<string, ParticleObjectPool>();

    private Transform _parent;

    public ParticleSystemPooler(Transform parent)
    {
        _parent = parent;
    }

    public ParticlePoolObject GetFromPool(ParticlePoolObject particlePrefab)
    {
        string prefabName = particlePrefab.name;
        if (!_poolDict.ContainsKey(prefabName))
            _poolDict.Add(prefabName, new ParticleObjectPool(particlePrefab, _parent));

        return _poolDict[prefabName].Get();
    }

    public ParticlePoolObject GetFromPool(ParticlePoolObject particlePrefab, Vector2 pos, Quaternion quat)
    {
        string prefabName = particlePrefab.name;
        if (!_poolDict.ContainsKey(prefabName))
            _poolDict.Add(prefabName, new ParticleObjectPool(particlePrefab, _parent));

        ParticlePoolObject particle = _poolDict[prefabName].Get();
        particle.transform.SetPositionAndRotation(pos, quat);

        return particle;
    }

    public ParticlePoolObject GetFromPool(ParticlePoolObject particlePrefab, Vector2 pos, Quaternion quat, Transform parent)
    {
        string prefabName = particlePrefab.name;
        if (!_poolDict.ContainsKey(prefabName))
            _poolDict.Add(prefabName, new ParticleObjectPool(particlePrefab, _parent));

        ParticlePoolObject particle = _poolDict[prefabName].Get();
        particle.transform.SetParent(parent);
        particle.transform.SetPositionAndRotation(pos, quat);

        return particle;
    }

    public void ReturnToPool(ParticlePoolObject particle)
    {
        _poolDict[particle.name].Return(particle);
    }
}
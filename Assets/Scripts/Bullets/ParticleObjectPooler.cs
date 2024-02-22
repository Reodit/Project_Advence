using MathNet.Numerics.Statistics;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticleObjectPooler : GenericObjectPooler<ParticlePoolObject>
{
    private Dictionary<string, ParticleObjectPool> poolDict = new Dictionary<string, ParticleObjectPool>();

    public override ParticlePoolObject Instantiate(ParticlePoolObject particle, Vector3 pos, Quaternion quat)
    {
        if (particle == null)
            return null;

        string particleName = particle.name;

        if (!poolDict.ContainsKey(particleName))
            poolDict.Add(particleName, new ParticleObjectPool(particle));

        ParticlePoolObject newParticle = poolDict[particleName].Get();
        newParticle.transform.SetPositionAndRotation(pos, quat);

        return newParticle;
    }

    public override void ReturnToPool(ParticlePoolObject particle)
    {
        poolDict[particle.name].Release(particle);
    }
}

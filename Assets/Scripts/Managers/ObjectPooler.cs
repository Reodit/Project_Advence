using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    public BulletObjectPooler Bullet { get; private set; }
    public ParticleSystemPooler Particle { get; private set; }
    public MonsterObjectPooler Monster { get; private set; }

    private void Awake()
    {
        Instance = this;
        Bullet = new BulletObjectPooler(transform);
        Particle = new ParticleSystemPooler(transform);
    }

    public void WaitForDestroy(ParticlePoolObject particle, float waitTime)
    {
        StartCoroutine(CoWaitForDestroy(particle, waitTime));
    }

    private IEnumerator CoWaitForDestroy(ParticlePoolObject particle, float waitTime)
    {
        yield return new WaitForSeconds(waitTime);
        Particle.ReturnToPool(particle);
    }
}

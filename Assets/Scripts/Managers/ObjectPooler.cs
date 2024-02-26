using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum PoolObject
{
    Bullet,
    Particle,
    Monster
}

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    public BulletObjectPooler Bullet { get; private set; }
    public ParticleObjectPooler Particle { get; private set; }
    public MonsterObjectPooler Monster { get; private set; }

    private void Awake()
    {
        Instance = this;
        Bullet = new BulletObjectPooler(transform);
        Particle = new ParticleObjectPooler(transform);
        Monster = new MonsterObjectPooler(null);
    }

    public void WaitForDestroy<T>(T t, float waitTime) where T : IPooledObject
    {
        StartCoroutine(CoWaitForDestroy(t, waitTime));
    }

    private IEnumerator CoWaitForDestroy<T>(T t, float waitTime) where T : IPooledObject
    {
        yield return new WaitForSeconds(waitTime);
        switch (t)
        {
            case Bullet bullet:
                Bullet.ReturnToPool(t as Bullet);
                break;
            case ParticlePoolObject particle:
                Particle.ReturnToPool(t as ParticlePoolObject);
                break;
            case Monster mosnter:
                Monster.ReturnToPool(t as Monster);
                break;
        }
    }
}

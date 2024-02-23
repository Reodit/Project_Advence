using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }

    public BulletObjectPooler Bullet { get; private set; }
    public ParticleSystemPooler Particle { get; private set; }

    private void Awake()
    {
        Instance = this;
        Bullet = new BulletObjectPooler(transform);
        Particle = new ParticleSystemPooler(transform);
    }
}

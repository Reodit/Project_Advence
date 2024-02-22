using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectPooler : MonoBehaviour
{
    public static ObjectPooler Instance { get; private set; }    
    
    public BulletObjectPooler Bullet { get; private set; } = new BulletObjectPooler();
    public ParticleSystemPooler Particle { get; private set; } = new ParticleSystemPooler();

    private void Awake()
    {
        Instance = this;
    }
}

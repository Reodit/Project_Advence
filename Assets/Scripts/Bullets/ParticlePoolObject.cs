using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ParticlePoolObject : MonoBehaviour, IPooledObject
{
    public ParticleSystem Particle { get; private set; }

    private void Awake()
    {
        Particle = GetComponent<ParticleSystem>();
    }

    public void OnObjectDestroy()
    {
    }

    public void OnObjectInstantiate()
    {
    }

    public void OnObjectReturn()
    {
    }

    public void OnObjectSpawn()
    {
        
    }
}

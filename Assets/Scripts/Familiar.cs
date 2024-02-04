using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour
{
    protected float spawnCoolTime;
    protected BulletSpawner bulletSpawner;
    [SerializeField] private Familiar scale;

    protected Vector3 spawnPos;

    // bulletSpawn // + bullet cooltime
    // FamiliarSpawn // respawn cooltime
    // move transform
    // delete --> 2가지 타입에 규칙이 다름
    // combat

    protected void Update()
    {
        if (CheckDestroyCondition())
        {
            // Destroy;
        }
    }

    protected virtual void Start()
    {
        // Initialize
        // spawnPos
        // bulletSpawner / bullets
        // spawnCoolTime
        // scale
    }

    protected void OnDestroy()
    {
        
    }
    
    protected virtual bool CheckDestroyCondition()
    {
        return false;
    }
}

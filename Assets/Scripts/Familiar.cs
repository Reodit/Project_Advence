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
        Init();
    }

    protected virtual void Init()
    {
    }

    protected void OnDestroy()
    {
        
    }
    
    protected virtual bool CheckDestroyCondition()
    {
        return false;
    }
}

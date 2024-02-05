using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Familiar : MonoBehaviour
{
    protected float spawnCoolTime;
    protected BulletController bulletController;
    [SerializeField] private Familiar scale;

    protected Vector3 spawnPos;

    protected virtual void Update()
    {
        if (CheckDestroyCondition())
        {
            // Destroy;
        }
    }

    protected virtual void Start()
    {
        // Initialize
        spawnPos = GameManager.Instance.PlayerMove.transform.position;
        // bulletSpawner / bullets
        // spawnCoolTime
        
        Init();
    }

    protected virtual void Init()
    {
    }

    protected void OnDestroy()
    {
        
    }
    
    public virtual void HitMonster(Monster monster)
    {
        
    }
    
    protected virtual bool CheckDestroyCondition()
    {
        return false;
    }
}

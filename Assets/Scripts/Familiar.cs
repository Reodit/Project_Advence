using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum FamiliarType
{
    melee,
    range
}

public class Familiar : MonoBehaviour
{
    public SkillTable familiarSkillData;
    public FamiliarData familiarData;
    public FamiliarType FamiliarType;
    public float spawnCoolTime;
    public int currentHp;
    public BulletController bulletController;
    [SerializeField] private Vector3 scale;

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

    public virtual void ApplyEnchant()
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

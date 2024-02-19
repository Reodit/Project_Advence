using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Serialization;

public class Familiar : MonoBehaviour
{
    public SkillTable familiarSkillData;
    public FamiliarData familiarData;
    public float currentHp;
    public BulletController bulletController;
    [SerializeField] private Vector3 scale;

    protected Animator animator;

    protected virtual void Update()
    {
        if (CheckDestroyCondition())
        {
            Destroy(this.gameObject);
        }
    }

    protected virtual void Start()
    {
        // Initialize
        // bulletSpawner / bullets
        // spawnCoolTime
        
        Init();
    }
    
    protected virtual void Init()
    {
    }

    protected virtual void OnDestroy()
    {
        
    }

    public virtual void ApplyEnchant(SkillEnchantTable skillEnchant)
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

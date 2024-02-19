using System;
using UnityEngine;

[Serializable]
public struct BulletInfo
{
    [field: SerializeField] public float Damage;
    [field: SerializeField] public float SkillSpeedRate;
    [field: SerializeField] public float MaxDistance;
    [field: SerializeField] public float Speed; // 총알의 속도
    
    public BulletInfo(float damage, float skillSpeedRate, float maxDistance, float speed)
    {
        Damage = damage;
        SkillSpeedRate = skillSpeedRate;
        MaxDistance = maxDistance;
        Speed = speed;
    }

    public void SetDamage(float damage)
    {
        Damage = damage;
    }

    public void SetSkillSpeedRate(float skillSpeedRate)
    {
        SkillSpeedRate = skillSpeedRate;
    }

    public void SetMaxDistance(float maxDistance)
    {
        MaxDistance = maxDistance;
    }

    public void SetSpeed(float speed)
    {
        Speed = speed;
    }
}

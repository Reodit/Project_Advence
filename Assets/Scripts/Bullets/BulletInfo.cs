using System;
using UnityEngine;
using UnityEngine.Serialization;

[Serializable]
public struct BulletInfo
{
    [field: SerializeField] public string bulletPrefabPath;
    [field: SerializeField] public float damage;
    [field: SerializeField] public float skillSpeedRate;
    [field: SerializeField] public float maxDistance;
    [field: SerializeField] public float speed; // 총알의 속도
    
    public BulletInfo(string bulletPrefabPath, float damage, float skillSpeedRate, float maxDistance, float speed)
    {
        this.bulletPrefabPath = bulletPrefabPath;
        this.damage = damage;
        this.skillSpeedRate = skillSpeedRate;
        this.maxDistance = maxDistance;
        this.speed = speed;
    }

    public void SetDamage(float damage)
    {
        this.damage = damage;
    }

    public void SetSkillSpeedRate(float skillSpeedRate)
    {
        this.skillSpeedRate = skillSpeedRate;
    }

    public void SetMaxDistance(float maxDistance)
    {
        this.maxDistance = maxDistance;
    }

    public void SetSpeed(float speed)
    {
        this.speed = speed;
    }
}

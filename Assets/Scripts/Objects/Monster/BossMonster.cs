using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossMonster : Monster
{
    [SerializeField] private float triggerCooldown = 0.5f;
    private string _monsterAttackCoolTimeID;

    protected override void Start()
    {
        base.Start();
        _monsterAttackCoolTimeID = "MonsterAttack_" + gameObject.GetInstanceID();
        TimeManager.Instance.RegisterCoolTime(_monsterAttackCoolTimeID, triggerCooldown);
    }

    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (TimeManager.Instance.IsCoolTimeFinished(_monsterAttackCoolTimeID))
        {
            CollisionManager.Instance.HandleCollision(this.gameObject, other.gameObject);
            TimeManager.Instance.Use(_monsterAttackCoolTimeID);
        }
    }
}

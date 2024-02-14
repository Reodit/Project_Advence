using System.Collections;
using System.Collections.Generic;
using FSM;
using Managers;
using UnityEngine;
using UnityEngine.Serialization;

public class RangeMonster : Monster
{
    [SerializeField] private float triggerCooldown = 0.5f;
    private string _monsterMeleeAttackCoolTimeID;
    private string _monsterRangeAttackCoolTimeID;
    [SerializeField] private Transform bulletStartPoint;
    [SerializeField] private Bullet monsterBullet;
    protected override void Start()
    {
        base.Start();
        _monsterMeleeAttackCoolTimeID = "MonsterMeleeAttack_" + gameObject.GetInstanceID();
        _monsterRangeAttackCoolTimeID = "MonsterRangeAttack_" + gameObject.GetInstanceID();
        TimeManager.Instance.RegisterCoolTime(_monsterMeleeAttackCoolTimeID, triggerCooldown);
        TimeManager.Instance.RegisterCoolTime(_monsterRangeAttackCoolTimeID, 2f);
        monsterBullet.SetBulletInfo(new BulletInfo(10f, 3f, 15f, -5f));
    }
    protected override void InitializeFsm()
    {
        base.InitializeFsm();
        StateMachine = new StateMachine<Monster>(this);
        StateMachine.Init();
        var idle = StateMachine.CreateState(new MonsterIdle("MonsterIdle", true));
        var rangeAttack = StateMachine.CreateState(new MonsterRangeAttack("MonsterRangeAttack", true));
        var die = StateMachine.CreateState(new MonsterDie("MonsterDie", true));

        var idleToRangeAttackTransition = StateMachine.CreateTransition("MonsterIdleToRangeAttack", idle, rangeAttack);
        var idleToDieTransition = StateMachine.CreateTransition("MonsterIdleToDie", idle, die);
        var rangeAttackToIdleTransition = StateMachine.CreateTransition("MonsterRangeAttackToIdle", rangeAttack, idle);
        var rangeAttackToIDieTransition = StateMachine.CreateTransition("MonsterRangeAttackToDie", rangeAttack, die); 
        
        StateMachine.CurrentState = idle;
        
        TransitionParameter dieParam = new TransitionParameter("isDie", ParameterType.Bool);
        StateMachine.AddTransitionCondition(idleToDieTransition, 
            dieParam, targetValue => (bool)targetValue);
        StateMachine.AddTransitionCondition(rangeAttackToIDieTransition, 
            dieParam, targetValue => (bool)targetValue);

        TransitionParameter rangeAttackParam = new TransitionParameter("isRangeAttack", ParameterType.Bool);
        StateMachine.AddTransitionCondition(idleToRangeAttackTransition,
            rangeAttackParam, targetValue => (bool)targetValue);
        StateMachine.AddTransitionCondition(rangeAttackToIdleTransition,
            rangeAttackParam, targetValue => !(bool)targetValue);

        StateMachineManager.Instance.Register(gameObject.GetInstanceID(), StateMachine);
    }
    protected override void OnTriggerStay2D(Collider2D other)
    {
        if (TimeManager.Instance.IsCoolTimeFinished(_monsterMeleeAttackCoolTimeID))
        {
            CollisionManager.Instance.HandleCollision(this.gameObject, other.gameObject);
            TimeManager.Instance.Use(_monsterMeleeAttackCoolTimeID);
        }
    }

    public override void RangeAttack()
    {
        base.RangeAttack();

        if (TimeManager.Instance.IsCoolTimeFinished(_monsterRangeAttackCoolTimeID))
        {
            Animator.SetBool("isRangeAttack", true);
            Instantiate(monsterBullet, bulletStartPoint);
            TimeManager.Instance.Use(_monsterRangeAttackCoolTimeID);
        }
        else
        {
            Animator.SetBool("isRangeAttack", false);
        }
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        StateMachineManager.Instance.Unregister(gameObject.GetInstanceID());
    }
}

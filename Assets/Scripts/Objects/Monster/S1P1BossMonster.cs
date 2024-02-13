using System.Collections;
using System.Collections.Generic;
using FSM;
using Managers;
using UnityEngine;

public class S1P1BossMonster : Monster
{
    [SerializeField] private float triggerCooldown = 0.5f;
    private string _monsterMeleeAttackCoolTimeID;

    protected override void Start()
    {
        base.Start();
        _monsterMeleeAttackCoolTimeID = "MonsterMeleeAttack_" + gameObject.GetInstanceID();
        TimeManager.Instance.RegisterCoolTime(_monsterMeleeAttackCoolTimeID, triggerCooldown);
    }

    protected override void InitializeFsm()
    {
        base.InitializeFsm();
        StateMachine = new StateMachine<Monster>(this);
        var idle = StateMachine.CreateState(new MonsterIdle("MonsterIdle", true));
        var rangeAttack = StateMachine.CreateState(new MonsterIdle("MonsterRangeAttack", true));
        var die = StateMachine.CreateState(new MonsterDie("MonsterDie", true));

        var idleToRangeAttackTransition = StateMachine.CreateTransition("MonsterIdleToRangeAttack", idle, rangeAttack);
        var idleToDieTransition = StateMachine.CreateTransition("MonsterIdleToDie", idle, die);
        var rangeAttackToIdleTransition = StateMachine.CreateTransition("MonsterRangeAttackToIdle", rangeAttack, idle);
        var rangeAttackToIDieTransition = StateMachine.CreateTransition("MonsterRangeAttackToDie", rangeAttack, die);

        StateMachine.CurrentState = idle;

        TransitionParameter dieParam = new TransitionParameter("IsDie", ParameterType.Bool);
        StateMachine.AddTransitionCondition(idleToDieTransition,
            dieParam, targetValue => (bool)targetValue);
        StateMachine.AddTransitionCondition(rangeAttackToIDieTransition,
            dieParam, targetValue => (bool)targetValue);

        TransitionParameter rangeAttackParam = new TransitionParameter("isRangeAttack", ParameterType.Bool);
        StateMachine.AddTransitionCondition(idleToRangeAttackTransition,
            dieParam, targetValue => (bool)targetValue);
        StateMachine.AddTransitionCondition(rangeAttackToIdleTransition,
            dieParam, targetValue => !(bool)targetValue);

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
}

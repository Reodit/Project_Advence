using System;
using System.Collections.Generic;
using FSM;
using Managers;
using UnityEngine;

[Serializable]
public struct S1P1BossMonsterStateChangePercentValue
{
    public float value;
    public S1P1BossMonsterState bossState;
}

public enum S1P1BossMonsterState
{
    ChasePlayer,
    MoveAndRangeAttack
}

public class S1P1BossMonster : Monster
{
    [Header("Pattern Values")] 
    public List<S1P1BossMonsterStateChangePercentValue> stateSwitchValue;
    public float chaseMoveSpeed;
    public float chaseDurationTime;
    public int stateSwitchRangeAttackCount;
    
    [SerializeField] private float triggerCooldown = 0.5f;
    private string _monsterMeleeAttackCoolTimeID;

    protected override void Start()
    {
        base.Start();
        _monsterMeleeAttackCoolTimeID = "MonsterMeleeAttack_" + gameObject.GetInstanceID();
        TimeManager.Instance.RegisterCoolTime(_monsterMeleeAttackCoolTimeID, triggerCooldown);
        stateSwitchValue = new List<S1P1BossMonsterStateChangePercentValue>();
    }

    protected override void InitializeFsm()
    {
        base.InitializeFsm();
        StateMachine = new StateMachine<Monster>(this);
        StateMachine.Init();
        var idle = StateMachine.CreateState(new S1P1BossMonsterIdle("S1P1BossMonsterIdle", true));
        var move = StateMachine.CreateState(new S1P1BossMonsterPlayerChase("S1P1BossMonsterPlayerChase", true));
        var meleeAttack = StateMachine.CreateState(new MonsterMeleeAttack("MonsterMeleeAttack", true));
        var moveAndRangeAttack = StateMachine.CreateState(new S1P1BossMonsterMoveAndRangeAttack("S1P1BossMonsterMoveAndRangeAttack", true));
        var die = StateMachine.CreateState(new MonsterDie("MonsterDie", true));

        var idleToRangeAttackTransition = StateMachine.CreateTransition("MonsterIdleToRangeAttack", idle, moveAndRangeAttack);
        var idleToDieTransition = StateMachine.CreateTransition("MonsterIdleToDie", idle, die);
        var rangeAttackToIdleTransition = StateMachine.CreateTransition("MonsterRangeAttackToIdle", moveAndRangeAttack, idle);
        var rangeAttackToIDieTransition = StateMachine.CreateTransition("MonsterRangeAttackToDie", moveAndRangeAttack, die);

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
    
    protected override void OnDestroy()
    {
        base.OnDestroy();
        StateMachineManager.Instance.Unregister(gameObject.GetInstanceID());
    }
}

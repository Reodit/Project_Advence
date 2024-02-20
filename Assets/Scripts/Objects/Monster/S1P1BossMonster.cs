using System;
using System.Collections;
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
    public float meleeAttackThreshold;
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
        var chaseAndMeleeAttack = StateMachine.CreateState(new S1P1BossMonsterPlayerChase("S1P1BossMonsterChaseAndMeleeAttack", true));
        var moveAndRangeAttack = StateMachine.CreateState(new S1P1BossMonsterMoveAndRangeAttack("S1P1BossMonsterMoveAndRangeAttack", true));
        var die = StateMachine.CreateState(new MonsterDie("MonsterDie", true));

        StateMachine.CurrentState = idle;
        
        // Die
        StateMachine.AddGlobalCondition(() => this.CurrentHp <= 0, () => {StateMachine.ChangeState(die);});
        
        var idleToChaseAndMeleeAttackTransition = StateMachine.CreateTransition("IdleToChaseAndMeleeAttackTransition", idle, chaseAndMeleeAttack);
        var idleToMoveAndRangeAttackTransition = StateMachine.CreateTransition("IdleToMoveAndRangeAttackTransition", idle, moveAndRangeAttack);
        var chaseAndMeleeAttackToIdleTransition = StateMachine.CreateTransition("ChaseAndMeleeAttackToIdleTransition", chaseAndMeleeAttack, idle);
        var moveAndRangeAttackTransitionToIdleTransition = StateMachine.CreateTransition("MoveAndRangeAttackTransitionToIdleTransition", moveAndRangeAttack, idle);
        
        TransitionParameter chaseAndMeleeAttackParam = new TransitionParameter(
            "ChaseAndMeleeAttackParam", ParameterType.Bool);
        StateMachine.AddTransitionCondition(idleToChaseAndMeleeAttackTransition,
            chaseAndMeleeAttackParam, targetValue => (bool)targetValue);
        StateMachine.AddTransitionCondition(chaseAndMeleeAttackToIdleTransition,
            chaseAndMeleeAttackParam, targetValue => (bool)targetValue);

        TransitionParameter moveAndRangeAttackParam = new TransitionParameter(
            "MoveAndRangeAttackParam", ParameterType.Bool);
        StateMachine.AddTransitionCondition(idleToMoveAndRangeAttackTransition,
            moveAndRangeAttackParam, targetValue => (bool)targetValue);
        StateMachine.AddTransitionCondition(moveAndRangeAttackTransitionToIdleTransition,
            moveAndRangeAttackParam, targetValue => !(bool)targetValue);

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

    public void StartActionCoroutine(IEnumerator coroutine)
    {
        StartCoroutine(coroutine);
    }
    public IEnumerator MeleeAttack()
    {
        Animator.Play("AttackMagic");

        yield return new WaitUntil(() =>
            Animator.GetCurrentAnimatorStateInfo(0).normalizedTime >= 1);
    }

    public IEnumerator MoveTowardCo(Vector2 targetPosition, float threshold, float moveSpeed)
    {
        float distance = Vector2.Distance(transform.position, targetPosition);
        while (distance > threshold) 
        {
            base.MoveToward(targetPosition, threshold, moveSpeed);
        }


        yield return new WaitUntil(() => distance < threshold);
    }
}

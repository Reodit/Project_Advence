using System;
using System.Collections;
using System.Collections.Generic;
using FSM;
using Managers;
using Unity.Mathematics;
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
    [Header("BossMonster Values")] 
    public float baseMoveSpeed;

    [Header("Pattern Values")] 
    public List<S1P1BossMonsterStateChangePercentValue> stateSwitchValue;
    public float chaseMoveSpeed;
    public float chaseDurationTime;
    [HideInInspector] public float meleeAttackThreshold = 0.3f;
    public int stateSwitchRangeAttackCount;
    public List<Vector2> wayPoint;
    public bool IsRunCoroutine { get; set; }
    private string _monsterMeleeAttackCoolTimeID;

    private int _currentRangeAttackCount;
    [Header("Attack Pattern Values")]
    [SerializeField] private float triggerCooldown = 0.5f;
    [SerializeField] private Transform bulletStartPoint;
    [SerializeField] private Bullet monsterBullet;
    protected override void Start()
    {
        base.Start();
        _monsterMeleeAttackCoolTimeID = "MonsterMeleeAttack_" + gameObject.GetInstanceID();
        TimeManager.Instance.RegisterCoolTime(_monsterMeleeAttackCoolTimeID, triggerCooldown);
        _currentRangeAttackCount = 0;
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
        yield return StartCoroutine(MoveTowardCo(
            new Vector2(4.8f, 0.8f), 0.1f, baseMoveSpeed));
        
        StateMachine.ChangeState(StateMachine.GetState("S1P1BossMonsterIdle"));
    }
    
    public override void HitPlayer(PlayerMove playerMove)
    {
        base.HitPlayer(playerMove);
        Animator.Play("AttackMagic");
        if (!IsRunCoroutine)
        {
            IsRunCoroutine = true;
            StartCoroutine(MeleeAttack());
        }
    }
    
    public IEnumerator MoveTowardCo(Vector2 targetPosition, float threshold, float moveSpeed)
    {
        float distance = Vector2.Distance(transform.position, targetPosition);
        while (distance > threshold) 
        {
            MoveToward(targetPosition, threshold, moveSpeed);
            distance = Vector2.Distance(transform.position, targetPosition);
            yield return null;
        }
        StateMachine.ChangeState(StateMachine.GetState("S1P1BossMonsterIdle"));
    }
    
    private int _waypointIndex = 0;

    public IEnumerator MoveThroughWaypoints(float threshold, float moveSpeed)
    {
        Debug.Log("Enter");
        
        
        while (true)
        {
            yield return StartCoroutine(MoveOneTapCo(wayPoint[_waypointIndex], threshold, moveSpeed));
            yield return StartCoroutine(RangeAttackCo());
            
            _waypointIndex = (_waypointIndex + 1) % wayPoint.Count;
            
            if (stateSwitchRangeAttackCount <= _currentRangeAttackCount)
            {
                _currentRangeAttackCount = 0;
                StateMachine.ChangeState(StateMachine.GetState("S1P1BossMonsterIdle"));
                yield break;
            }
        }
    }

    public IEnumerator MoveOneTapCo(Vector2 targetPosition, float threshold, float moveSpeed)
    {
        float distance = Vector2.Distance(transform.position, targetPosition);
        while (distance > threshold)
        {
            MoveToward(targetPosition, threshold, moveSpeed);
            distance = Vector2.Distance(transform.position, targetPosition);
            yield return null;
        }
    }

    public void InstantiateProjectile()
    {
        Instantiate(monsterBullet, bulletStartPoint.position, quaternion.identity);
        _currentRangeAttackCount++;
    }
    
    public IEnumerator RangeAttackCo()
    {
        Animator.Play("AttackBow");
        yield return new WaitForSeconds(1f);
    }
}

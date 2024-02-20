using System.Linq;
using FSM;
using FSM.States;
using UnityEngine;

public class MonsterIdle : IState<Monster>
{
    public string StateName { get; set; }
    public void Enter(Monster owner)
    {
    }

    public void Execute(Monster owner)
    {
        if (owner.CurrentHp <= 0)
        {
            owner.StateMachine.SetBool("isDie", true);
            owner.Animator.SetBool("isDie", true);
            owner.GetComponent<Collider2D>().isTrigger = false;
        }
        
        // TODO 수정 필요 (임시처리)
        if (CameraUtility.IsTargetInCameraView(
                GameManager.Instance.mainCamera, owner.gameObject.transform.position
            ))
        {
            owner.RangeAttack();
        }
    }

    public void Exit(Monster owner)
    {
        // Debug.Log("MonsterIdle.Exit");
    }

    public bool RequiresUpdate { get; set; }

    public MonsterIdle(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}

public class MonsterMeleeAttack : IState<Monster>
{
    public string StateName { get; set; }
    public void Enter(Monster owner)
    {
        // Debug.Log("MonsterMeleeAttack.Enter");
    }

    public void Execute(Monster owner)
    {

        
    }

    public void Exit(Monster owner)
    {
        // Debug.Log("MonsterMeleeAttack.Exit");
    }

    public bool RequiresUpdate { get; set; }
    
    public MonsterMeleeAttack(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}

public class MonsterRangeAttack : IState<Monster>
{
    public string StateName { get; set; }
    public void Enter(Monster owner)
    {
        // Debug.Log("MonsterRangeAttack.Enter");
    }

    public void Execute(Monster owner)
    {
        // Debug.Log("MonsterRangeAttack.Execute");
    }

    public void Exit(Monster owner)
    {
        // Debug.Log("MonsterRangeAttack.Exit");
    }

    public bool RequiresUpdate { get; set; }
    
    public MonsterRangeAttack(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}

public class MonsterDie : IState<Monster>
{
    public string StateName { get; set; }
    public void Enter(Monster owner)
    {
        owner.GetComponent<Collider2D>().isTrigger = false;
        owner.Animator.SetBool("isDie", true);
        // Debug.Log("MonsterDie.Enter");
    }

    public void Execute(Monster owner)
    {
        // Debug.Log("MonsterDie.Execute");
        var currentAnimatorState = owner.Animator.GetCurrentAnimatorStateInfo(0);
        if (currentAnimatorState.IsName("Die") && currentAnimatorState.normalizedTime > 1)
        {
            owner.StateMachine.ChangeState(new NullState<Monster>());
        }
    }

    public void Exit(Monster owner)
    {
        // Debug.Log("MonsterDie.Exit");
        owner.Die();
    }

    public bool RequiresUpdate { get; set; }
    
    public MonsterDie(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}

public class S1P1BossMonsterIdle : IState<Monster>
{
    public string StateName { get; set; }
    public void Enter(Monster owner)
    {
        var s1P1BossMonster = owner as S1P1BossMonster;
        s1P1BossMonster!.IsRunCoroutine = false;
    }

    public void Execute(Monster owner)
    {
        Debug.Log("S1P1BossMonsterIdle.Execute");

        float randomValue = Random.Range(0f, 1f) * 100;
        var s1P1BossMonster = owner as S1P1BossMonster;
        if (s1P1BossMonster == null)
        {
            return;
        }
        
        float accumulatedProbability = 0;

        foreach (var stateSwitch in s1P1BossMonster.stateSwitchValue)
        {
            accumulatedProbability += stateSwitch.value;
            if (randomValue <= accumulatedProbability)
            {
                switch (stateSwitch.bossState)
                {
                    case S1P1BossMonsterState.ChasePlayer:
                        owner.StateMachine.ChangeState(owner.StateMachine.GetState(
                            "S1P1BossMonsterChaseAndMeleeAttack"));
                        break;
                    
                    case S1P1BossMonsterState.MoveAndRangeAttack:
                        owner.StateMachine.ChangeState(owner.StateMachine.GetState(
                            "S1P1BossMonsterMoveAndRangeAttack"));
                        break;
                }
                break;
            }
        }
    }

    public void Exit(Monster owner)
    {
        // Debug.Log("MonsterIdle.Exit");
    }

    public bool RequiresUpdate { get; set; }

    public S1P1BossMonsterIdle(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}

public class S1P1BossMonsterPlayerChase : IState<Monster>
{
    public string StateName { get; set; }
    public void Enter(Monster owner)
    {
        var s1P1BossMonster = owner as S1P1BossMonster;
        if (s1P1BossMonster != null)
        {
            TimeManager.Instance.RegisterCoolTime(
                s1P1BossMonster.gameObject.GetInstanceID() + StateName,
                s1P1BossMonster.chaseDurationTime);
            
        }
    }

    public void Execute(Monster owner)
    {
        var s1P1BossMonster = owner as S1P1BossMonster;
        if (s1P1BossMonster == null)
        {
            return;
        }

        if (s1P1BossMonster.IsRunCoroutine)
        {
            return;
        }
        
        var playerMove = GameManager.Instance.PlayerMove;
        Debug.Log("S1P1BossMonsterPlayerChase.Execute");
        if (owner.MoveToward(playerMove.transform.position,
                s1P1BossMonster.meleeAttackThreshold, s1P1BossMonster.chaseMoveSpeed))
        {
            Debug.Log("MoveTowardCo");
            s1P1BossMonster.StartCoroutine(s1P1BossMonster.MeleeAttack());
            s1P1BossMonster.IsRunCoroutine = true;
        }

        if (s1P1BossMonster.IsRunCoroutine)
        {
            return;
        }
        
        if (TimeManager.Instance.IsCoolTimeFinished(
                s1P1BossMonster.gameObject.GetInstanceID() + StateName))
        {
            s1P1BossMonster.StartCoroutine(s1P1BossMonster.MoveTowardCo(
                new Vector2(4.8f, 0.8f), 0.05f, s1P1BossMonster.baseMoveSpeed));
            s1P1BossMonster.IsRunCoroutine = true;
            TimeManager.Instance.Use(s1P1BossMonster.gameObject.GetInstanceID() + StateName);
        }
    }

    public void Exit(Monster owner)
    {
        // Debug.Log("MonsterIdle.Exit");
        var s1P1BossMonster = owner as S1P1BossMonster;
        s1P1BossMonster!.IsRunCoroutine = false;
    }

    public bool RequiresUpdate { get; set; }

    public S1P1BossMonsterPlayerChase(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}

public class S1P1BossMonsterMoveAndRangeAttack : IState<Monster>
{
    public string StateName { get; set; }
    public void Enter(Monster owner)
    {
        var s1P1BossMonster = owner as S1P1BossMonster;
        s1P1BossMonster!.IsRunCoroutine = false;
        
    }

    public void Execute(Monster owner)
    {
        Debug.Log("S1P1BossMonsterMoveAndRangeAttack.Execute");
        
        var s1P1BossMonster = owner as S1P1BossMonster;
        if (s1P1BossMonster == null)
        {
            return;
        }
        
        if (!s1P1BossMonster.IsRunCoroutine)
        {
            s1P1BossMonster.StartCoroutine(s1P1BossMonster.MoveThroughWaypoints(
            0.01f, s1P1BossMonster.baseMoveSpeed));
            s1P1BossMonster.IsRunCoroutine = true;   
        }
    }

    public void Exit(Monster owner)
    {        
        var s1P1BossMonster = owner as S1P1BossMonster;
        s1P1BossMonster!.IsRunCoroutine = false;
    }

    public bool RequiresUpdate { get; set; }

    public S1P1BossMonsterMoveAndRangeAttack(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}
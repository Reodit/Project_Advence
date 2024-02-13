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
        if (owner.CurrentHp <= 0)
        {
            owner.StateMachine.SetBool("isDie", true);
            owner.Animator.SetBool("isDie", true);
            owner.GetComponent<Collider2D>().isTrigger = false;
        }
        
        
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

public class BossMonsterIdle : IState<S1P1BossMonster>
{
    public string StateName { get; set; }
    public void Enter(S1P1BossMonster owner)
    {
    }

    public void Execute(S1P1BossMonster owner)
    {
        if (owner.CurrentHp <= 0)
        {
            owner.StateMachine.SetBool("isDie", true);
            owner.Animator.SetBool("isDie", true);
            owner.GetComponent<Collider2D>().isTrigger = false;
        }
        
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= 0.3f)
        {
            // 캐릭터 추적
        }
        
        else
        {
            // 위로 1칸 이동
        }
    }

    public void Exit(S1P1BossMonster owner)
    {
        // Debug.Log("MonsterIdle.Exit");
    }

    public bool RequiresUpdate { get; set; }

    public BossMonsterIdle(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}

public class BossMonsterMove : IState<S1P1BossMonster>
{
    public string StateName { get; set; }
    public void Enter(S1P1BossMonster owner)
    {
    }

    public void Execute(S1P1BossMonster owner)
    {
        if (owner.CurrentHp <= 0)
        {
            owner.StateMachine.SetBool("isDie", true);
            owner.Animator.SetBool("isDie", true);
            owner.GetComponent<Collider2D>().isTrigger = false;
        }
        
        float randomValue = Random.Range(0f, 1f);

        if (randomValue <= 0.3f)
        {
            // 캐릭터 추적
        }
        
        else
        {
            // 위로 1칸 이동
        }
    }

    public void Exit(S1P1BossMonster owner)
    {
        // Debug.Log("MonsterIdle.Exit");
    }

    public bool RequiresUpdate { get; set; }

    public BossMonsterMove(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}
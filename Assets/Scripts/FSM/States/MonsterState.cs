using FSM;
using UnityEngine;

public class MonsterIdle : IState<Monster>
{
    public string StateName { get; set; }
    public void Enter(Monster owner)
    {
        Debug.Log("MonsterIdle.Enter");
    }

    public void Execute(Monster owner)
    {
        Debug.Log("MonsterIdle.Execute");
    }

    public void Exit(Monster owner)
    {
        Debug.Log("MonsterIdle.Exit");
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
        Debug.Log("MonsterMeleeAttack.Enter");
    }

    public void Execute(Monster owner)
    {
        Debug.Log("MonsterMeleeAttack.Execute");
    }

    public void Exit(Monster owner)
    {
        Debug.Log("MonsterMeleeAttack.Exit");
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
        Debug.Log("MonsterRangeAttack.Enter");
    }

    public void Execute(Monster owner)
    {
        Debug.Log("MonsterRangeAttack.Execute");
    }

    public void Exit(Monster owner)
    {
        Debug.Log("MonsterRangeAttack.Exit");
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
        Debug.Log("MonsterDie.Enter");
    }

    public void Execute(Monster owner)
    {
        Debug.Log("MonsterDie.Execute");
    }

    public void Exit(Monster owner)
    {
        Debug.Log("MonsterDie.Exit");
    }

    public bool RequiresUpdate { get; set; }
    
    public MonsterDie(string stateName, bool requiresUpdate)
    {
        this.StateName = stateName;
        this.RequiresUpdate = requiresUpdate;
    }
}
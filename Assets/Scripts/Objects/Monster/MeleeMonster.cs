using System.Collections;
using System.Collections.Generic;
using FSM;
using Managers;
using UnityEngine;

public class MeleeMonster : Monster
{
    [SerializeField] private float triggerCooldown = 0.5f;
    private string _monsterAttackCoolTimeID;

    protected override void Start()
    {
        base.Start();
        _monsterAttackCoolTimeID = "MonsterAttack_" + gameObject.GetInstanceID();
        TimeManager.Instance.RegisterCoolTime(_monsterAttackCoolTimeID, triggerCooldown);
    }

    protected override void InitializeFsm()
    {
        base.InitializeFsm();
        StateMachine = new StateMachine<Monster>(this);
        var idle = StateMachine.CreateState(new MonsterIdle("MonsterIdle", true));
        var die = StateMachine.CreateState(new MonsterDie("MonsterDie", true));

        var idleToDieTransition = StateMachine.CreateTransition("MonsterIdleToDie", idle, die);
        var dieToIdleTransition = StateMachine.CreateTransition("MonsterDieToIdle", die, idle);
            
        StateMachine.CurrentState = idle;
        
        TransitionParameter dieParam = new TransitionParameter("Die", ParameterType.Bool);
        StateMachine.AddTransitionCondition(idleToDieTransition, 
            dieParam, targetValue => (bool)targetValue);
        StateMachine.AddTransitionCondition(dieToIdleTransition, 
            dieParam, targetValue => !(bool)targetValue);
        
        StateMachineManager.Instance.Register(gameObject.GetInstanceID(), StateMachine);
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

using FSM;

public class BossMonsterState : IState<BossMonster>
{
    public string StateName { get; set; }
    public void Enter(BossMonster owner)
    {
        throw new System.NotImplementedException();
    }

    public void Execute(BossMonster owner)
    {
        throw new System.NotImplementedException();
    }

    public void Exit(BossMonster owner)
    {
        throw new System.NotImplementedException();
    }

    public bool RequiresUpdate { get; set; }
}

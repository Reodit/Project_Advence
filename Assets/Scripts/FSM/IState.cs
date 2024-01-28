namespace FSM
{
    public interface IState<T>
    {
        public string StateName { get; set; }
        public void Enter(T owner);
        public void Execute(T owner);
        public void Exit(T owner);
        public bool RequiresUpdate { get; set; }
    }
}
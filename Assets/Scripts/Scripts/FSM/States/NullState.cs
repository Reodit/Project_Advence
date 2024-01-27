namespace FSM.States
{
    public class NullState<T> : IState<T>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get => false; set { } }

        public void Enter(T owner) { }

        public void Execute(T owner) { }

        public void Exit(T owner) { }
    }
}

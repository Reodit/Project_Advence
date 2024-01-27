namespace FSM.States
{
    public class ItemIdle : IState<Item>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get => false; set { } }

        public ItemIdle(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }
        
        public void Enter(Item owner) { }

        public void Execute(Item owner) { }

        public void Exit(Item owner) { }
    }
    
    public class ItemInteraction : IState<Item>
    {
        public string StateName { get; set; }
        public bool RequiresUpdate { get => false; set { } }

        public ItemInteraction(string stateName, bool requiresUpdate)
        {
            StateName = stateName;
            RequiresUpdate = requiresUpdate;
        }
        public void Enter(Item owner) { }

        public void Execute(Item owner) { }

        public void Exit(Item owner) { }
    }
}

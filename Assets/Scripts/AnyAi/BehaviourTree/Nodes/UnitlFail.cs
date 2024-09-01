namespace BehaviourTree
{
    public class UntilFall : Node
    {
        public override BTNodeStatus Status { get; protected set; }
        public sealed override string Name { get; protected set; }
        
        protected UntilFall(string name, int priority, IBTDebugger debugger) : base(debugger)
        {
            Name = name;
            Priority = priority;
        }
        public override BTNodeStatus Process()
        {
            if (_nodes[0].Process() == BTNodeStatus.Failure)
            {
                Reset();
                Status = BTNodeStatus.Failure;
                Debug(this);
                return Status;
            }

            return Status = BTNodeStatus.Running;
        }

        public override void AddChild(INode node) => _nodes.Add(node);
    }
}
namespace BehaviourTree
{
    public class Selector : Node
    {
        public override BTNodeStatus Status { get; protected set; }
        public sealed override string Name { get; protected set; }

        protected Selector(string name, int priority, IBTDebugger debugger) : base(debugger)
        {
            Name = name;
            Priority = priority;
        }

        public override BTNodeStatus Process()
        {
            if (CurrentChild < Nodes.Count)
            {
                Debug(this);
                switch (Nodes[CurrentChild].Process())
                {
                    case BTNodeStatus.Running : return Status = BTNodeStatus.Running;
                    case BTNodeStatus.Success : Reset();
                        return Status = BTNodeStatus.Success;
                    default: CurrentChild++;
                        return Status = BTNodeStatus.Running;
                }
            }
            Reset();
            return Status = BTNodeStatus.Failure;
        }

        public override void AddChild(INode node) => _nodes.Add(node);
    }
}
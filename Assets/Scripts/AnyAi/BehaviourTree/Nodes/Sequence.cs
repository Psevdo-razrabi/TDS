namespace BehaviourTree
{
    public class Sequence : Node
    {
        public override BTNodeStatus Status { get; protected set; }
        public sealed override string Name { get; protected set; }

        public Sequence(string name, int priority, IBTDebugger debugger) : base(debugger)
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
                    case BTNodeStatus.Running: return Status = BTNodeStatus.Running;
                    case BTNodeStatus.Failure: Reset();
                        return Status = BTNodeStatus.Failure;
                    default: CurrentChild++;
                        return Status = CurrentChild == _nodes.Count ? BTNodeStatus.Success : BTNodeStatus.Running;
                }
            }
            
            Reset();
            return Status = BTNodeStatus.Success;
        }


        public override void AddChild(INode node) => _nodes.Add(node);
    }
}
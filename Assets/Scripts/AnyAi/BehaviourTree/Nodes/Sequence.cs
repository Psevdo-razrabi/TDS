namespace BehaviourTree
{
    public class Sequence : Node
    {
        public override BTNodeStatus Status { get; protected set; }
        public sealed override string Name { get; protected set; }

        public Sequence(string name, int cost, IBTDebugger debugger) : base(cost, debugger)
        {
            Name = name;
        }

        public override BTNodeStatus Process()
        {
            if (CurrentChild < Nodes.Count)
            {
                switch (Nodes[CurrentChild].Process())
                {
                    case BTNodeStatus.Running: return Status = BTNodeStatus.Running;
                    case BTNodeStatus.Failure: Stop();
                        return Status = BTNodeStatus.Failure;
                    default: CurrentChild++;
                        return Status = CurrentChild == _nodes.Count ? BTNodeStatus.Success : BTNodeStatus.Running;
                }
            }
            
            Stop();
            return Status = BTNodeStatus.Success;
        }

        public override void Start()
        {
            base.Start();
            Debug(this, Name);
        }


        public override void AddChild(INode node) => _nodes.Add(node);
    }
}
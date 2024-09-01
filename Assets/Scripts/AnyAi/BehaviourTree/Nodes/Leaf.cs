using Customs;

namespace BehaviourTree
{
    public class Leaf : Node
    {
        public override BTNodeStatus Status { get; protected set; }
        public sealed override string Name { get; protected set; }
        private readonly IStrategy _action;
        
        public override void AddChild(INode node) => _nodes.Add(node);

        public override BTNodeStatus Process()
        {
            Status = _action.Process();
            Debug(this);
            return Status;
        }

        public override void Reset() => _action.Reset();

        public Leaf(IBTDebugger debugger, IStrategy action, string name, int priority) : base(debugger)
        {
            Preconditions.CheckNotNull(action);
            Name = name;
            _action = action;
            Priority = priority;
        }
    }
}
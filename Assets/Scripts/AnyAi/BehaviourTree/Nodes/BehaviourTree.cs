using System.Text;

namespace BehaviourTree
{
    public class BehaviourTree : Node
    {
        public override BTNodeStatus Status { get; protected set; }
        public sealed override string Name { get; protected set; }

        public BehaviourTree(string name, IBTDebugger debugger) : base(debugger)
        {
            Name = name;
        }

        public override BTNodeStatus Process()
        {
            while (CurrentChild < Nodes.Count)
            {
                Debug(this);
                Status = _nodes[CurrentChild].Process();

                if (Status != BTNodeStatus.Success) return Status;

                CurrentChild++;
            }

            return Status = BTNodeStatus.Success;
        }

        public override void AddChild(INode node) => _nodes.Add(node);
    }
}
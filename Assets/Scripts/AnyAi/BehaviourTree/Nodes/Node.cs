using System.Collections.Generic;

namespace BehaviourTree
{
    public abstract class Node : INode, IDebuggable
    {
        public abstract string Name { get; protected set; }
        public IReadOnlyList<INode> Nodes => _nodes;
        public abstract BTNodeStatus Status { get; protected set; }
        public int CurrentChild { get; protected set; }
        public int Priority { get; protected set; }
        public IBTDebugger Debugger { get; }
        protected readonly List<INode> _nodes = new();
        public abstract void AddChild(INode node);

        public virtual BTNodeStatus Process()
        {
            Status = Nodes[CurrentChild].Process();
            Debug(this);
            return Status;
        }
        
        public virtual void Reset()
        {
            CurrentChild = 0;
            foreach (var node in _nodes)
            {
                node.Reset();
            }

        }

        protected Node(IBTDebugger debugger) => Debugger = debugger;
        protected void Debug<T>(T node)
        {
            Debugger.NodeStatus.Value = Status;
            Debugger.NameNode.Value = Name;
            Debugger.TypeNode.Value = node.GetType().ToString();
        }
        
    }
}

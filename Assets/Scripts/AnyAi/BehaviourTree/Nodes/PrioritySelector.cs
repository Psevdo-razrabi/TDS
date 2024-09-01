using System.Collections.Generic;
using System.Linq;

namespace BehaviourTree
{
    public class PrioritySelector : Selector
    {
        public override BTNodeStatus Status { get; protected set; }
        private IReadOnlyList<INode> SortedChildren => _sortedChildren ??= SortChildren();
        private List<INode> _sortedChildren;

        public PrioritySelector(string name, int priority, IBTDebugger debugger) : base(name, priority, debugger)
        {
            Name = name;
            Priority = priority;
        }

        public override void AddChild(INode node) => _nodes.Add(node);
        
        public override void Reset()
        {
            base.Reset();
            _sortedChildren = null;
        }

        public override BTNodeStatus Process()
        {
            foreach (var sortChild in SortedChildren)
            {
                Debug(this);
                switch (sortChild.Process())
                {
                    case BTNodeStatus.Running:
                        return Status = BTNodeStatus.Running;
                    case BTNodeStatus.Success:
                        return Status = BTNodeStatus.Success;
                    default: continue;
                }
            }

            return Status = BTNodeStatus.Failure;
        }

        protected virtual List<INode> SortChildren()
        {
            return _nodes.OrderByDescending(child => child.Priority).ToList();
        }
    }
}
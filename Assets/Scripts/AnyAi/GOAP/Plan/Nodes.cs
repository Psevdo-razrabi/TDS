using System.Collections.Generic;

namespace GOAP.Plan
{
    public class Node
    {
        public Node Parent { get; }
        public AgentAction AgentAction { get; }
        public HashSet<AgentBelief> RequiredEffects { get; }
        public List<Node> Leaves { get; } = new();
        public float Cost { get; }

        public bool IsLeafDead => Leaves.Count == 0 && AgentAction == null;
        
        public Node(Node parent, AgentAction agentAction, HashSet<AgentBelief> requiredEffects, float cost)
        {
            Parent = parent;
            AgentAction = agentAction;
            RequiredEffects = new HashSet<AgentBelief>(requiredEffects);
            Cost = cost;
        }
    }
}
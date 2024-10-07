using System.Collections.Generic;
using Customs;
using GOAP;
using UnityEngine;

namespace BehaviourTree
{
    public class Leaf : Node
    {
        public override BTNodeStatus Status { get; protected set; }
        public sealed override string Name { get; protected set; }
        public bool IsLeafDead => Nodes.Count == 0 && AgentAction == null;
        public readonly AgentAction AgentAction;
        public readonly HashSet<AgentBelief> RequiredEffects;
        
        public override void AddChild(INode node) => _nodes.Add(node);

        public override BTNodeStatus Process()
        {
            DebugStatus();
            Status = AgentAction.Update(Time.deltaTime);
            return Status;
        }

        public override void Stop() => AgentAction.Stop();

        public override void Start()
        {
            Debug(this, Name);
            AgentAction.Start();
        }

        public Leaf(AgentAction agentAction, HashSet<AgentBelief> requiredEffects, float cost, string name, IBTDebugger btDebugger) : base(cost, btDebugger)
        {
            AgentAction = agentAction;
            RequiredEffects = new HashSet<AgentBelief>(requiredEffects);
            Name = name;
        }
    }
}
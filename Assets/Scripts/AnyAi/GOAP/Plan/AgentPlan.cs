using System.Collections.Generic;
using BehaviourTree;

namespace GOAP
{
    public class AgentPlan
    {
        public Stack<Leaf> Actions { get; }
        public float TotalCost { get; set; }
        
        public AgentPlan(float totalCost, Stack<Leaf> actions)
        {
            TotalCost = totalCost;
            Actions = actions;
        }
    }
}
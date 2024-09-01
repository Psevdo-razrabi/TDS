using System.Collections.Generic;

namespace GOAP.Plan
{
    public class AgentPlan
    {
        public AgentGoal AgentGoal { get; }
        public Stack<AgentAction> Actions { get; }
        public float TotalCost { get; set; }
        
        public AgentPlan(float totalCost, Stack<AgentAction> actions, AgentGoal agentGoal)
        {
            TotalCost = totalCost;
            Actions = actions;
            AgentGoal = agentGoal;
        }
    }
}
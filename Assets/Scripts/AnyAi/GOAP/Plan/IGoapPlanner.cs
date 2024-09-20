using System.Collections.Generic;

namespace GOAP
{
    public interface IGoapPlanner
    {
        (AgentPlan plan, AgentGoal goal) GetPlan(GoapAgent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal);
    }
}
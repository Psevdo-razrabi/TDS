using System.Collections.Generic;

namespace GOAP.Plan
{
    public interface IGoapPlanner
    {
        AgentPlan GetPlan(GoapAgent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal);
    }
}
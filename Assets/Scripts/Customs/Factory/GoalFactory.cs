using System.Collections.Generic;
using GOAP;

namespace CharacterOrEnemyEffect.Factory
{
    public class GoalFactory
    {
        private readonly HashSet<AgentGoal> _agentGoals;

        public GoalFactory(HashSet<AgentGoal> agentGoals)
        {
            _agentGoals = agentGoals;
        }


        public void AddGoalAgent(string key, int priority, AgentBelief agentBelief)
        {
            var goal = new GoalBuilder(key);
            
            _agentGoals
                .Add(goal
                .WithPriority(priority)
                .WithDesiredEffect(agentBelief)
                .BuildGoal());
        }
    }
}
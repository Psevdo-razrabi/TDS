using System.Collections.Generic;
using System.Linq;

namespace GOAP
{
    public class CancelGoal
    {
        private HashSet<AgentGoal> _agentGoals;
        private HashSet<AgentAction> _agentActions;

        public CancelGoal(HashSet<AgentGoal> agentGoals, HashSet<AgentAction> agentActions)
        {
            _agentGoals = agentGoals;
            _agentActions = agentActions;
        }

        public void CancelCurrentStateAndComputeNewPlan(BehaviourTree.BehaviourTree currentTree, AgentGoal currentGoal = null)
        {
            if (!FindMorePriorityGoal(currentGoal)) return;
            currentTree.Stop();
            currentTree.Reset();
        }
        
        private bool FindMorePriorityGoal(AgentGoal currentGoal = null)
        {
            var orderedGoals = _agentGoals
                .Where(goal => goal.DesiredEffects.Any(belief => !belief.CheckCondition()))
                .Where(goal => goal.Priority > currentGoal?.Priority)
                .OrderByDescending(goal => goal.Priority);

            var actionsList = (from action in _agentActions from goal in orderedGoals where action.Effects.Any(goal.DesiredEffects.Contains) select action)
                .ToList();

            return actionsList.Any(CanExecuteAction);
        }
        
        private bool CanExecuteAction(AgentAction action)
        {
            var preconditionsMet = action.IsActionToBeUse();
            return preconditionsMet;
        }
    }
}
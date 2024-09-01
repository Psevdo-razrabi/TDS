using System.Collections.Generic;
using System.Linq;
using UnityEngine;

namespace GOAP.Plan
{
    public class GoapPlanner : IGoapPlanner
    {
        public AgentPlan GetPlan(GoapAgent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null)
        {
            var orderedGoals = goals
                .Where(goal => goal.DesiredEffects.Any(belief => !belief.CheckCondition()))
                .OrderByDescending(goal => goal == mostRecentGoal? goal.Priority - 0.01 : goal.Priority)
                .ToList();


            foreach (var goal in orderedGoals)
            {
                var goalNode = new Node(null, null, goal.DesiredEffects, 0);

                if (FindPath(goalNode, agent._action))
                {
                    if(goalNode.IsLeafDead) continue;

                    var actionStack = new Stack<AgentAction>();
                    while (goalNode.Leaves.Count > 0)
                    {
                        var cheapestLeaf = goalNode.Leaves.OrderBy(leaf => leaf.Cost).First();

                        goalNode = cheapestLeaf;
                        actionStack.Push(cheapestLeaf.AgentAction);
                    }

                    return new AgentPlan(goalNode.Cost, actionStack, goal);
                }
            }
            Debug.LogWarning("No plan found");
            return null;
        }

        private bool FindPath(Node node, HashSet<AgentAction> agentActions)
        {
            var sortByCost = agentActions.OrderBy(action => action.Cost);
            
            foreach (var action in sortByCost)
            {
                var requiredEffects = node.RequiredEffects;

                requiredEffects.RemoveWhere(belief => belief.CheckCondition());

                if (requiredEffects.Count == 0) return true;

                if (action.Effects.Any(requiredEffects.Contains))
                {
                    var newRequiredEffects = new HashSet<AgentBelief>(requiredEffects);
                    newRequiredEffects.ExceptWith(action.Effects);
                    newRequiredEffects.UnionWith(action.Precondition);

                    var newAvailableActions = new HashSet<AgentAction>(agentActions);
                    newAvailableActions.Remove(action);

                    
                    var newNode = new Node(node, action, newRequiredEffects, node.Cost + action.Cost);

                    if (FindPath(newNode, newAvailableActions))
                    {
                        node.Leaves.Add(newNode);
                        newRequiredEffects.ExceptWith(newNode.AgentAction.Precondition);
                    }

                    if (newRequiredEffects.Count == 0) return true;
                }
            }

            return false;
        }
        
    }
}
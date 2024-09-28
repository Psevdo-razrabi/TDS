using System.Collections.Generic;
using System.Linq;
using BehaviourTree;
using UnityEngine;

namespace GOAP
{
    public class GoapPlanner : IGoapPlanner
    {
        private IBTDebugger _debugger;

        public GoapPlanner(IBTDebugger debugger)
        {
            _debugger = debugger;
        }

        public (AgentPlan plan, AgentGoal goal) GetPlan(GoapAgent agent, HashSet<AgentGoal> goals, AgentGoal mostRecentGoal = null)
        {
            var orderedGoals = goals
                .Where(goal => goal.DesiredEffects.Any(belief => !belief.CheckCondition()))
                .OrderByDescending(goal => goal == mostRecentGoal ? goal.Priority - 0.01 : goal.Priority);
            
            foreach (var goal in orderedGoals)
            {
                var goalNode = new Leaf(null, goal.DesiredEffects, 0f, "Nothing", _debugger);

                if (FindPath(goalNode, agent._action))
                {
                    if(goalNode.IsLeafDead) continue;

                    var actionStack = new Stack<Leaf>();
                    while (goalNode.Nodes.Count > 0)
                    {
                        var cheapestLeaf = goalNode.Nodes.OrderBy(leaf => leaf.Cost).First();

                        goalNode = (Leaf)cheapestLeaf;
                        actionStack.Push((Leaf)cheapestLeaf);
                    }

                    return (new AgentPlan(goalNode.Cost, actionStack), goal);
                }
            }
            Debug.LogWarning("No plan found");
            return (null, null);
        }

        private bool FindPath(Leaf node, HashSet<AgentAction> agentActions)
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
                    
                    var newNode = new Leaf(action, newRequiredEffects, node.Cost + action.Cost, action.Name, _debugger);

                    if (FindPath(newNode, newAvailableActions))
                    {
                        node.AddChild(newNode);
                        newRequiredEffects.ExceptWith(newNode.AgentAction.Precondition);
                    }

                    if (newRequiredEffects.Count == 0) return true;
                }
            }

            return false;
        }
        
    }
}
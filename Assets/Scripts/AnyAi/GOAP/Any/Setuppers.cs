using System.Collections.Generic;
using CharacterOrEnemyEffect.Factory;

namespace GOAP
{
    public class Setuppers
    {
        private readonly HashSet<AgentAction> _actions;
        private readonly Dictionary<string, AgentBelief> _agentBeliefs;
        private readonly HashSet<AgentGoal> _goals;
        private readonly GoapAgent _goapAgent;

        private StrategyFactory _strategyFactory = new();
        
        public Setuppers(HashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs, HashSet<AgentGoal> goals, GoapAgent goapAgent)
        {
            _actions = actions;
            _agentBeliefs = agentBeliefs;
            _goals = goals;
            _goapAgent = goapAgent;
        }
        
        public void SetupGoals()
        {
            var factory = new GoalFactory(_goals);
            
            factory.AddGoalAgent("Idle", 1, _agentBeliefs["Nothing"]);
            factory.AddGoalAgent("Walking", 1, _agentBeliefs["AgentMoving"]);
            factory.AddGoalAgent("Health", 2, _agentBeliefs["AgentIsHealthy"]);
            factory.AddGoalAgent("Stamin", 2, _agentBeliefs["AgentIsRested"]);
            factory.AddGoalAgent("Attack", 3, _agentBeliefs["AttackingPlayer"]);
        }

        public void SetupActions()
        {
            _actions.Add(new ActionBuilder("Chill")
                .WithActionStrategy(_strategyFactory.CreateIdleStrategy(3f, _goapAgent.transform))
                .WithEffect(_agentBeliefs["Nothing"])
                .BuildAgentAction());
            
            _actions.Add(new ActionBuilder("Walk")
                .WithActionStrategy(_strategyFactory.CreatePatrolStrategy(_goapAgent.patrolPoints, _goapAgent._navMeshAgent, _goapAgent.transform, 15f))
                .WithEffect(_agentBeliefs["AgentMoving"])
                .BuildAgentAction());

            _actions.Add(new ActionBuilder("MoveToEat")
                .WithActionStrategy(_strategyFactory.CreateMoveToPointStrategy(_goapAgent._navMeshAgent, () => _goapAgent.foodCort.transform.position))
                .WithEffect(_agentBeliefs["AgentAtFoodPosition"])
                .BuildAgentAction());

            _actions.Add(new ActionBuilder("Heal")
                .WithActionStrategy(_strategyFactory.CreateIdleStrategy(5f, _goapAgent.transform))
                .WithPrecondition(_agentBeliefs["AgentAtFoodPosition"])
                .WithEffect(_agentBeliefs["AgentIsHealthy"])
                .BuildAgentAction());
            
            _actions.Add(new ActionBuilder("PlayerLook")
                .WithActionStrategy(_strategyFactory.CreateMoveToPointStrategy(_goapAgent._navMeshAgent, () => _agentBeliefs["PlayerInEyeSensor"].Location))
                .WithPrecondition(_agentBeliefs["PlayerInEyeSensor"])
                .WithEffect(_agentBeliefs["AttackingPlayer"])
                .BuildAgentAction());

            _actions.Add(new ActionBuilder("PlayerAttack")
                .WithActionStrategy(_strategyFactory.CreateAttackStrategy())
                .WithPrecondition(_agentBeliefs["PlayerInEyeSensor"])
                .WithEffect(_agentBeliefs["AttackingPlayer"])
                .BuildAgentAction());
        }

        public void SetupBeliefs()
        {
            var factory = new BeliefFactory(_agentBeliefs);
            
            factory.AddBeliefCondition("Nothing", () => false);
            factory.AddBeliefCondition("AgentIdle", () => _goapAgent._navMeshAgent.hasPath == false);
            factory.AddBeliefCondition("AgentMoving", () => _goapAgent._navMeshAgent.hasPath);
            
            //TEST
            factory.AddBeliefCondition("AgentIsHealthLow", () => _goapAgent._health < 30);
            factory.AddBeliefCondition("AgentIsHealthy", () => _goapAgent._health >= 50);
            factory.AddBeliefCondition("AgentStaminaLow", () => _goapAgent._stamina < 10);
            factory.AddBeliefCondition("AgentIsRested", () => _goapAgent._stamina >= 50);
            
            factory.AddLocationBelief("AgentAtFoodPosition", _goapAgent.foodCort.transform.position, () => !_goapAgent.InRangeOf(_goapAgent.foodCort.transform.position, 3f));
            factory.AddLocationBelief("AgentAtRestingPosition", _goapAgent.chilZone.transform.position, () => !_goapAgent.InRangeOf(_goapAgent.chilZone.transform.position, 3f));
            
            factory.AddSensorBelief("PlayerInEyeSensor", _goapAgent._eyesSensor);
            factory.AddBeliefCondition("AttackingPlayer", () => false);
        }
    }
}
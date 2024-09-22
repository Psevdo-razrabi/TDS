using System.Collections.Generic;
using BlackboardScripts;
using CharacterOrEnemyEffect.Factory;
using Game.Player.PlayerStateMashine;

namespace GOAP
{
    public class Setuppers
    {
        private readonly HashSet<AgentAction> _actions;
        private readonly Dictionary<string, AgentBelief> _agentBeliefs;
        private readonly HashSet<AgentGoal> _goals;
        private readonly GoapAgent _goapAgent;
        private readonly BlackboardController _blackboard;

        private StrategyFactory _strategyFactory = new();
        
        public Setuppers(HashSet<AgentAction> actions, Dictionary<string, AgentBelief> agentBeliefs,
            HashSet<AgentGoal> goals, GoapAgent goapAgent, BlackboardController blackboardController)
        {
            _actions = actions;
            _agentBeliefs = agentBeliefs;
            _goals = goals;
            _goapAgent = goapAgent;
            _blackboard = blackboardController;
        }
        
        public void SetupGoals()
        {
            var factory = new GoalFactory(_goals);
            
            factory.AddGoalAgent("Idle", 1, _agentBeliefs["Nothing"]);
            factory.AddGoalAgent("Walking", 1, _agentBeliefs["AgentMoving"]);
            factory.AddGoalAgent("Health", 2, _agentBeliefs["AgentIsHealthy"]);
            factory.AddGoalAgent("Attack", 3, _agentBeliefs["AttackingPlayer"]);
        }

        public void SetupActions()
        {
            _actions.Add(new ActionBuilder("Chill")
                .WithActionStrategy(_strategyFactory.CreateIdleStrategy(3f, _blackboard))
                .WithEffect(_agentBeliefs["Nothing"])
                .WithPreconditionUse(() => HasPath() == false)
                .BuildAgentAction());
            
            _actions.Add(new ActionBuilder("Walk")
                .WithActionStrategy(_strategyFactory.CreatePatrolStrategy(_blackboard, 10f))
                .WithEffect(_agentBeliefs["AgentMoving"])
                .WithPreconditionUse(HasPath)
                .BuildAgentAction());
 
            _actions.Add(new ActionBuilder("MoveToEat")
                .WithActionStrategy(_strategyFactory.CreateMoveToPointStrategy(_blackboard, () => _goapAgent.foodCort.transform.position))
                .WithEffect(_agentBeliefs["AgentAtFoodPosition"])
                .BuildAgentAction());

            _actions.Add(new ActionBuilder("Heal")
                .WithActionStrategy(_strategyFactory.CreateIdleStrategy(5f, _blackboard))
                .WithPrecondition(_agentBeliefs["AgentAtFoodPosition"])
                .WithEffect(_agentBeliefs["AgentIsHealthy"])
                .WithPreconditionUse(() => HasHealth() < 30)
                .BuildAgentAction());
            
            _actions.Add(new ActionBuilder("PlayerLook")
                .WithActionStrategy(_strategyFactory.CreateMoveToPointStrategy(_blackboard, () => _agentBeliefs["PlayerInEyeSensor"].Location))
                .WithPrecondition(_agentBeliefs["PlayerInEyeSensor"])
                .WithEffect(_agentBeliefs["AttackingPlayer"])
                .WithPreconditionUse(() => HasSensor(NameExperts.EyesSensor).IsTargetInSensor)
                .BuildAgentAction());
            
            _actions.Add(new ActionBuilder("PlayerHit")
                .WithActionStrategy(_strategyFactory.CreateMoveToPointStrategy(_blackboard, () => _agentBeliefs["PlayerInHitSensor"].Location))
                .WithPrecondition(_agentBeliefs["PlayerInHitSensor"])
                .WithEffect(_agentBeliefs["AttackingPlayer"])
                .WithPreconditionUse(() => HasSensor(NameExperts.HitSensor).IsTargetInSensor)
                .BuildAgentAction());

            _actions.Add(new ActionBuilder("PlayerAttackAfterHit")
                .WithActionStrategy(_strategyFactory.CreateAttackStrategy())
                .WithPrecondition(_agentBeliefs["PlayerInHitSensor"])
                .WithPrecondition(_agentBeliefs["PlayerInEyeSensor"])
                .WithEffect(_agentBeliefs["AttackingPlayer"])
                .WithPreconditionUse(() => HasSensor(NameExperts.EyesSensor).IsTargetInSensor)
                .BuildAgentAction());
        }

        public void SetupBeliefs()
        {
            var factory = new BeliefFactory(_agentBeliefs);
            
            factory.AddBeliefCondition("Nothing", () => false);
            factory.AddBeliefCondition("AgentIdle", () => HasPath() == false);
            factory.AddBeliefCondition("AgentMoving", HasPath);
            
            factory.AddBeliefCondition("AgentIsHealthLow", () => HasHealth() < 30);
            factory.AddBeliefCondition("AgentIsHealthy", () => HasHealth() >= 50);
            
            factory.AddLocationBelief("AgentAtFoodPosition", _goapAgent.foodCort.transform.position, HasLocationFood);
            factory.AddLocationBelief("AgentAtRestingPosition", _goapAgent.chilZone.transform.position, HasLocationChill);
            
            factory.AddSensorBelief("PlayerInEyeSensor", HasSensor(NameExperts.EyesSensor));
            factory.AddSensorBelief("PlayerInHitSensor", HasSensor(NameExperts.HitSensor));
            factory.AddBeliefCondition("AttackingPlayer", () => false);
        }

        private bool HasPath()
        {
            return _blackboard.GetValue<bool>(NameExperts.Movement);
        }

        private float HasHealth()
        {
            return _blackboard.GetValue<float>(NameExperts.HealthStats);
        }

        private ISensor HasSensor(string nameSensor)
        {
            return _blackboard.GetValue<ISensor>(nameSensor);
        }

        private bool HasLocationFood()
        {
            return _blackboard.GetValue<bool>(NameExperts.LocationFood);
        }
        
        private bool HasLocationChill()
        {
            return _blackboard.GetValue<bool>(NameExperts.LocationChillZone);
        }
    }
}
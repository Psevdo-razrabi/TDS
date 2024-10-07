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
                .WithActionStrategy(_strategyFactory.CreatePatrolStrategy(_blackboard, 20f))
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
                .WithActionStrategy(_strategyFactory.CreateMoveAttack(_blackboard))
                .WithPrecondition(_agentBeliefs["PlayerInEyeSensor"])
                .WithEffect(_agentBeliefs["EnemySearch"])
                .WithPreconditionUse(() => HasSensor(NameExperts.EyesSensor).IsActivate.Value)
                .BuildAgentAction());
            
            _actions.Add(new ActionBuilder("PlayerHit")
                .WithActionStrategy(_strategyFactory.CreateMoveAttack(_blackboard))
                .WithPrecondition(_agentBeliefs["PlayerInHitSensor"])
                .WithEffect(_agentBeliefs["EnemySearch"])
                .WithPreconditionUse(() => HasSensor(NameExperts.HitSensor).IsActivate.Value)
                .BuildAgentAction());
            
            _actions.Add(new ActionBuilder("PlayerEscaped")
                .WithActionStrategy(_strategyFactory.CreateEnemySearch(_blackboard))
                .WithPrecondition(_agentBeliefs["EnemySearch"])
                .WithEffect(_agentBeliefs["PlayerToAttackSensor"])
                .BuildAgentAction());

            _actions.Add(new ActionBuilder("PlayerAttack")
                .WithActionStrategy(_strategyFactory.CreateAttackStrategy())
                .WithPrecondition(_agentBeliefs["PlayerToAttackSensor"])
                .WithEffect(_agentBeliefs["AttackingPlayer"])
                .WithPreconditionUse(() => HasSensor(NameExperts.AttackSensor).IsActivate.Value)
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
            factory.AddSensorBelief("PlayerToAttackSensor", HasSensor(NameExperts.AttackSensor));
            factory.AddBeliefCondition("AttackingPlayer", () => false);
            factory.AddBeliefCondition("EnemySearch", () => _blackboard.GetValue<bool>(NameAIKeys.EnemySearch));
        }

        private bool HasPath()
        {
            var isWhat = _blackboard.GetValue<bool>(NameExperts.Movement);
            return isWhat;
        }

        private float HasHealth()
        {
            var isWhat = _blackboard.GetValue<float>(NameExperts.HealthStats);
            return isWhat;
        }

        private ISensor HasSensor(string nameSensor)
        {
            var isWhat = _blackboard.GetValue<ISensor>(nameSensor);
            return isWhat;
        }

        private bool HasLocationFood()
        {
            var isWhat = _blackboard.GetValue<bool>(NameExperts.LocationFood);
            return isWhat;
        }
        
        private bool HasLocationChill()
        {
            var isWhat = _blackboard.GetValue<bool>(NameExperts.LocationChillZone);
            return isWhat;
        }
    }
}
using System;
using System.Collections.Generic;
using BehaviourTree;
using BlackboardScripts;
using BlackboardScripts.Expert;
using Game.Player.PlayerStateMashine;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Zenject;

namespace GOAP
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class GoapAgent : MonoBehaviour
    {
        public readonly HashSet<AgentAction> _action = new();
        
        [Header("Sensors")] 
        [SerializeField] public EyesSensor _eyesSensor;
        [SerializeField] public HitSensor _hitSensor;

        [Header("Locations")] 
        [SerializeField] public Transform foodCort;
        [SerializeField] public Transform chilZone;
        [SerializeField] public Transform[] patrolPoints;

        [Header("HealthStats")] 
        [SerializeField] public float _health;
        [SerializeField] public float _stamina;
        
        [Header("Goap Scripts")]
        private IGoapPlanner _goapPlanner;
        private AgentPlan _actionPlan;
        private Setuppers _setuppers;
        private BehaviourTree.BehaviourTree _behaviourTree;
        private BlackboardController _blackboardController;
        private AgentGoal _agentGoal;
        private RegisterExperts _registerExperts;
        private readonly Dictionary<string, AgentBelief> _agentBeliefs = new();
        private readonly HashSet<AgentGoal> _goals = new();
        private CancelGoal _cancelGoal;
        
        
        private CompositeDisposable _disposable = new();
        private IBTDebugger _debugger;

        [Inject]
        public void Construct(IBTDebugger debugger)
        {
            _debugger = debugger;
        }

        private void Awake()
        {
            _blackboardController = new BlackboardController();
            _blackboardController.Initialize();
            _registerExperts = new RegisterExperts(_blackboardController);
            _registerExperts.Initialize((currentHealth) => _health >= currentHealth, (currentHealth) => _health < currentHealth);
            SetupDataToBlackboard(GetComponent<Animator>(), GetComponent<NavMeshAgent>());
            _goapPlanner = new GoapPlanner(_debugger);
            _behaviourTree = new BehaviourTree.BehaviourTree("Agent Tree", 0, _debugger);
            _setuppers = new Setuppers(_action, _agentBeliefs, _goals, this, _blackboardController);
        }

        private void OnEnable()
        {
            SetupTimers();
        }

        private void OnDisable()
        {
            _disposable.Clear();
            _disposable.Dispose();
            _blackboardController.Dispose();
        }

        private void Start()
        {
            _setuppers.SetupBeliefs();
            _setuppers.SetupGoals();
            _setuppers.SetupActions();
            _cancelGoal = new CancelGoal(_goals, _action);
        }

        private void SetupDataToBlackboard(Animator animator, NavMeshAgent navMeshAgent)
        {
            _blackboardController.SetValue(NameAIKeys.Animator, animator);
            _blackboardController.SetValue(NameAIKeys.Agent, navMeshAgent);
            _blackboardController.SetValue(NameAIKeys.HealthAI, _health);
            _blackboardController.SetValue(NameAIKeys.FoodPoint, foodCort);
            _blackboardController.SetValue(NameAIKeys.PatrolPoints, patrolPoints);
            _blackboardController.SetValue(NameAIKeys.ChillPoint, chilZone);
            _blackboardController.SetValue(NameAIKeys.TransformAI, transform);
            _blackboardController.SetValue<Func<bool>>(NameExperts.MovementPredicate, 
                () => _blackboardController.GetValue<NavMeshAgent>(NameAIKeys.Agent).hasPath);
            _blackboardController.SetValue<Func<float>>(NameExperts.HealthStatsPredicate, 
                () => _health);
            _blackboardController.SetValue<Func<bool>>(NameExperts.StaminaStatsPredicate, 
                () => _stamina > 50);
            _blackboardController.SetValue<Func<bool>>(NameExperts.LocationFoodPredicate, 
                () => !InRangeOf(foodCort.transform.position, 3f));
            _blackboardController.SetValue<Func<bool>>(NameExperts.LocationChillZonePredicate, 
                () => !InRangeOf(chilZone.transform.position, 3f));
            _blackboardController.SetValue<ISensor>(NameExperts.EyesSensor, _eyesSensor);
            _blackboardController.SetValue<ISensor>(NameExperts.HitSensor, _hitSensor);
        }

        private void SetupTimers()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => EntryPoint())
                .AddTo(_disposable); 
            
            Observable
                .Timer(TimeSpan.FromSeconds(2f), TimeSpan.FromSeconds(2f))
                .Subscribe(_ => UpdateStats())
                .AddTo(_disposable);
        }

        private void UpdateStats()
        {
            _stamina += InRangeOf(chilZone.position, 3f) ? -10 : 20;
            _health += InRangeOf(foodCort.position, 2f) ? -10 : 20;

            _stamina = Math.Clamp(_stamina, 0f, 150f);
            _health = Math.Clamp(_health, 0f, 150f);
        }

        public bool InRangeOf(Vector3 pos, float range) => Vector3.Distance(transform.position, pos) > range;

        private void InitBehaviourTree(Stack<Leaf> leafs)
        {
            var sequencePlan = new Sequence("Sequence Leafs", 0, _debugger);

            for (int i = 0; i < leafs.Count; i++)
            {
                sequencePlan.AddChild(leafs.Pop());
            }
            
            _behaviourTree.AddChild(sequencePlan);
            _behaviourTree.Start();
        }

        private void EntryPoint()
        {
            if (_actionPlan == null)
            {
                CreatePlan();
            }

            if (_actionPlan != null)
            {
                _cancelGoal.CancelCurrentStateAndComputeNewPlan(_behaviourTree, _agentGoal);
            }
            
            CompletePlan();
        }
        
        private void CompletePlan()
        {
            _behaviourTree.Process();

            if (_behaviourTree.Status != BTNodeStatus.Success) return;
            
            _behaviourTree.Reset();
            _behaviourTree.SetGoalsState(null, _agentGoal);
            _actionPlan = null;
        }

        private void CreatePlan()
        {
            var goalsToCheck = _goals;
            var plan = _goapPlanner.GetPlan(this, goalsToCheck, _behaviourTree.LastGoal);
            if (plan.plan == null) return;
            
            _actionPlan = plan.plan;
            _agentGoal = plan.goal;
            _behaviourTree.SetGoalsState(_agentGoal, null);
            InitBehaviourTree(plan.plan.Actions);
        }
    }
}
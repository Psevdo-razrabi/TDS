using System;
using System.Collections.Generic;
using System.Linq;
using CharacterOrEnemyEffect.Factory;
using GOAP.Plan;
using UniRx;
using UnityEngine;
using UnityEngine.AI;

namespace GOAP
{
    [RequireComponent(typeof(NavMeshAgent))]
    [RequireComponent(typeof(Animator))]
    public class GoapAgent : MonoBehaviour
    {
        public readonly HashSet<AgentAction> _action = new();
        
        [Header("Sensor")] 
        [SerializeField] public EyesSensor _eyesSensor;

        [Header("Locations")] 
        [SerializeField] public Transform foodCort;
        [SerializeField] public Transform chilZone;
        [SerializeField] public Transform[] patrolPoints;

        [Header("Stats")] 
        [SerializeField] public float _health;
        [SerializeField] public float _stamina;
        
        private Animator _animator;
        public NavMeshAgent _navMeshAgent;
        private AgentGoal _lastGoal;
        private CompositeDisposable _disposable = new();
        private IGoapPlanner _goapPlanner = new GoapPlanner();

        private AgentGoal _currentGoal;
        private AgentPlan _actionPlan;
        private AgentAction _currentAction;

        private readonly Dictionary<string, AgentBelief> _agentBeliefs = new();
        private readonly HashSet<AgentGoal> _goals = new();
        private Setuppers _setuppers;


        private void Awake()
        {
            _animator = GetComponent<Animator>();
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _setuppers = new Setuppers(_action, _agentBeliefs, _goals, this);
        }

        private void OnEnable()
        {
            SetupTimers();
        }

        private void OnDisable()
        {
            _disposable.Clear();
            _disposable.Dispose();
        }

        private void Start()
        {
            _setuppers.SetupBeliefs();
            _setuppers.SetupGoals();
            _setuppers.SetupActions();
        }

        private void SetupTimers()
        {
            Observable
                .EveryUpdate()
                .Subscribe(_ => UpdatePlan())
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

        private void Reset()
        {
            _currentGoal = null;
            _currentAction = null;
        }

        private void UpdatePlan()
        {
            if (_currentAction == null)
            {
                Debug.LogWarning("Create a Plan");
                CreatePlan();
            
                if (_actionPlan != null && _actionPlan.Actions.Count > 0)
                {
                    _navMeshAgent.ResetPath();

                    _currentGoal = _actionPlan.AgentGoal;
                    _currentAction = _actionPlan.Actions.Pop();
                    _currentAction.Start();
                    
                    
                    Debug.LogWarning($"Goal: {_currentGoal} with {_actionPlan.Actions.Count}");
                    Debug.LogWarning($"Poppet action: {_currentAction.Name}");
                }
            }
            
            CompletePlan();
        }

        private void CompletePlan()
        {
            if (_actionPlan == null || _currentAction == null) return;
            _currentAction.Update(Time.deltaTime);

            if (_currentAction.Complete == false) return;
            _currentAction.Stop();
            _currentAction = null;

            if (_actionPlan.Actions.Count != 0) return;
            Debug.LogWarning("Plan complete");
            _lastGoal = _currentGoal;
            _currentGoal = null;
        }

        private void CreatePlan()
        {
            var priorityLevel = _currentGoal?.Priority ?? 0;
            var goalsToCheck = _goals;

            if (_currentGoal != null)
            {
                goalsToCheck = new HashSet<AgentGoal>(_goals.Where(goal => goal.Priority > priorityLevel));
            }
            
            var plan = _goapPlanner.GetPlan(this, goalsToCheck, _lastGoal);

            if (plan != null)
                _actionPlan = plan;
        }
    }
}
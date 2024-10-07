using BehaviourTree;
using BlackboardScripts;
using Customs;
using Game.Core.Health;
using Game.Player;
using Game.Player.AnyScripts;
using Game.Player.Interfaces;
using System.Collections.Generic;
using UI.Storage;
using UniRx;
using UnityEngine;
using UnityEngine.AI;
using Random = UnityEngine.Random;

namespace Enemy
{
    public class Enemy: MonoBehaviour, IHealth, IInitialaize, IHit, IInitializable<Enemy>
    {
        [SerializeField] private EnemyHealthConfig healthConfigConfig;
        [SerializeField] private RagdollHelper ragdollHelper;
        [SerializeField] private GameObject enemy;
        [SerializeField] private GameObject player;
        private NavMeshAgent _navMeshAgent;
        private List<Vector3> _points = new List<Vector3>();
        
        public IHealthStats HealthStats { get; private set; }
        public Subject<Unit> Hit { get; private set; } = new();
        private ValueCountStorage<float> _valueCountStorage;
        private BehaviourTree.BehaviourTree _behaviourTree;
        private IBTDebugger _debugger;
        private readonly Blackboard _blackboard = new();

        public void InitModel(ValueCountStorage<float> valueCountStorage)
        {
            _valueCountStorage = valueCountStorage;
        }

        public void Initialize()
        {
            var die = new Die(ragdollHelper);
            HealthStats = new Health<Enemy>(healthConfigConfig.MaxHealth, _valueCountStorage, die);
            var operationWithHealth = new OperationWithHealth<PlayerComponents>(die, this,
                HealthResoringConstyl._playerRestoring);
            
            operationWithHealth.SubscribeDead(operationWithHealth.EnemyDie);
            operationWithHealth.SubscribeHit(operationWithHealth.EnemyHitBullet);
        }

        public void Awake()
        {
            //AI start
            //var ground = GameObject.Find("Ground");
            //var navGrid = ground?.GetComponent<NavGrid>();
           // _generatorChunksRound = new GeneratorChunksRound(navGrid?.GetPointMap(), transform, 10);
            //AI  end
        }

        public void Start()
        {
            _blackboard.AddKeyValuePair("IsPlayerActive", new bool());
            _blackboard.SetValue(_blackboard.GetOrRegisterKey("IsPlayerActive"), player.activeSelf);
            _navMeshAgent = GetComponent<NavMeshAgent>();
            _points.Add(new Vector3(Random.Range(0f, 10f), 0f, Random.Range(0f, 10f)));
            _points.Add(new Vector3(Random.Range(-10f, 10f), 0f, Random.Range(-10f, 10f)));
            _points.Add(new Vector3(Random.Range(-15f, 15f), 0f, Random.Range(-15f, 15f)));
            
            //_behaviourTree = new BehaviourTree.BehaviourTree("Enemy", _debugger);
            
            /*Sequence sequence = new Sequence("", 20, _debugger);
            
            //_behaviourTree.AddChild(new Leaf("Patrul", new PatrolStrategy(transform, _navMeshAgent, _points, 2f)));
            Leaf isPlayerPresent = new Leaf(_debugger, new Conditions(() => PlayerIsActive(sequence)),"PlayerIsActive", 20);
            Leaf moveToPlayer = new Leaf(_debugger, new ActionStrategy(() => _navMeshAgent.SetDestination(player.transform.position)), "ToWalkPlayer", 20);
            
            Leaf isEnemyPresent = new Leaf(_debugger, new Conditions(() => enemy.activeSelf),"PlayerIsActive", 10);
            Leaf moveToEnemy = new Leaf(_debugger, new ActionStrategy(() => _navMeshAgent.SetDestination(enemy.transform.position)), "ToWalkPlayer", 10);
            
            sequence.AddChild(isPlayerPresent);
            sequence.AddChild(moveToPlayer);
            
            Sequence sequence1 = new Sequence("", 10, _debugger);
            sequence1.AddChild(isEnemyPresent);
            sequence1.AddChild(moveToEnemy);


            PrioritySelector goToPlayer = new PrioritySelector("MoveToPlayer", 10, _debugger);
            goToPlayer.AddChild(sequence);
            goToPlayer.AddChild(sequence1);*/
            
            //_behaviourTree.AddChild(goToPlayer);

        }

        public void Update()
        {
            //_behaviourTree.Process();

            //AI start
            //_generatorChunksRound.Update();
            //AI  end
    }

    public bool PlayerIsActive(Sequence sequence)
        {
            if (_blackboard.TryGetValue(_blackboard.GetOrRegisterKey("IsPlayerActive"), out bool active))
            {
                if(active == false)
                    sequence.Reset();
                return true;
            }

            return false;
        }
    }

    public interface IHit
    {
        Subject<Unit> Hit { get; }
    }
}
using Customs;
using Enemy;
using Game.Core.Health;
using Game.Player.AnimatorScripts;
using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using Input;
using PhysicsWorld;
using UI.Storage;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour, IStateDataWorker, IHealth, IInitialaize, IGravity
    {
        public InputSystemMovement InputSystem { get; private set; }
        public InputSystemMouse InputSystemMouse { get; private set; }
        public IPlayerAim PlayerAim { get; private set; }
        public AnimatorController AnimatorController { get; private set; }
        public CharacterController CharacterController { get; private set; }
        public IHealthStats HealthStats { get; private set; }
        [Inject] public PlayerConfigs PlayerConfigs { get; private set; }
        [Inject] public StateHandleChain StateChain { get; private set; }
        [Inject] public StateMachineData StateMachineData { get; private set; }
        [Inject] public AsyncWorker.AsyncWorker AsyncWorker { get; private set; }
        private ValueCountStorage<float> ValueModelHealth { get; set; }
        [Inject] public EventController EventController { get; private set; }
        [field: SerializeField] public GameObject PlayerModelRotate { get; private set; } 
        public DashTrailEffect DashTrailEffect { get; private set; }

        private InitializationStateMachine _initializationStateMachine;
        private CompositeDisposable _disposable = new();
        [SerializeField] private RagdollHelper ragdollHelper;

        [Inject]
        private void Construct(IPlayerAim playerAim, InputSystemMovement inputSystemMovement, 
            InputSystemMouse inputSystemMouse, AnimatorController animatorController, 
            InitializationStateMachine stateMachine, DashTrailEffect trailEffect)
        {
            PlayerAim = playerAim;
            InputSystem = inputSystemMovement;
            InputSystemMouse = inputSystemMouse;
            AnimatorController = animatorController;
            _initializationStateMachine = stateMachine;
            DashTrailEffect = trailEffect;
        }

        private async void Start()
        {
            CharacterController = GetComponent<CharacterController>();
            await AsyncWorker.AwaitLoadPlayerConfig(PlayerConfigs);
            StateMachineData.DashCount = PlayerConfigs.DashConfig.NumberChargesDash;
            
            HealthStats =
                new RestoringHealth(
                    new Health<Player>(PlayerConfigs.HealthConfig.MaxHealth, ValueModelHealth, 
                        new Die<Player>(gameObject, EventController, ragdollHelper)),
                    PlayerConfigs.HealthConfig, EventController, ValueModelHealth);
            
            HealthStats.Subscribe();
        }

        private void Update()
        {
            if(!_initializationStateMachine.PlayerStateMachine.isUpdate) return;
            
            _initializationStateMachine.PlayerStateMachine.currentStates.OnUpdateBehaviour();
        }

        private void FixedUpdate()
        {
            _initializationStateMachine.PlayerStateMachine.currentStates.OnFixedUpdateBehaviour();
        }

        private void OnDisable()
        {
            HealthStats.Unsubscribe();
            _disposable.Clear();
            _disposable.Dispose();
        }
        
        public void InitModel(ValueCountStorage<float> valueCountStorage)
        {
            ValueModelHealth = valueCountStorage;
        }
    }
}
using Customs;
using Enemy;
using Game.Core.Health;
using Game.Player.AnimatorScripts;
using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using Input;
using UI.Storage;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour, IStateDataWorker, IHealth, IInitialaize
    {
        [SerializeField] private RagdollHelper ragdollHelper;
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
         public ValueCountStorage<float> ValueModelHealth { get; private set; }
        [Inject] public EventController EventController { get; private set; }

        private InitializationStateMachine _initializationStateMachine;
        private CompositeDisposable _disposable = new();

        [Inject]
        private void Construct(IPlayerAim playerAim, InputSystemMovement inputSystemMovement, 
            InputSystemMouse inputSystemMouse, AnimatorController animatorController, 
            InitializationStateMachine stateMachine)
        {
            PlayerAim = playerAim;
            InputSystem = inputSystemMovement;
            InputSystemMouse = inputSystemMouse;
            AnimatorController = animatorController;
            _initializationStateMachine = stateMachine;
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
            if (UnityEngine.Input.GetKeyDown(KeyCode.G))
            {
                HealthStats.SetDamage(10f);
            }
            
            if(!_initializationStateMachine.PlayerStateMachine.isUpdate) return;
            
            _initializationStateMachine.PlayerStateMachine.currentStates.OnUpdateBehaviour();
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
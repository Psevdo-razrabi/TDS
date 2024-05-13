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
    public class Player : MonoBehaviour, IStateDataWorker, IHealth
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
        [Inject] public ValueCountStorage<float> ValueModelHealth { get; private set; }
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
            await AsyncWorker.Await(PlayerConfigs);
            StateMachineData.DashCount = PlayerConfigs.DashConfig.NumberChargesDash;
            
            HealthStats =
                new RestoringHealth(
                    new Health<Player>(PlayerConfigs.HealthConfig.MaxHealth, ValueModelHealth, 
                        new Die<Player>(gameObject, EventController)),
                    PlayerConfigs.HealthConfig, EventController, ValueModelHealth);
        }

        private void Update()
        {
            TestHealth();
            if(!_initializationStateMachine.PlayerStateMachine.isUpdate) return;
            
            _initializationStateMachine.PlayerStateMachine.currentStates.OnUpdateBehaviour();
        }


        private void TestHealth()
        {
            if (UnityEngine.Input.GetKeyDown(KeyCode.D))
            {
                HealthStats.SetDamage(10f);
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.H))
            {
                if (HealthStats is IHealthRestoring healthStats)
                {
                    switch (healthStats.IsHealthRestoringAfterHitEnemy)
                    {
                        case true:
                            HealthStats.CancellationTokenSource.Cancel();
                            break;
                        case false:
                            HealthStats.AddHealth(0f);
                            break;
                    }
                }
            }

            if (UnityEngine.Input.GetKeyDown(KeyCode.E))
            {
                var b = HealthStats as IHealthRestoring;
                b.IsHealthRestoringAfterDieEnemy = true;

                HealthStats.AddHealth(0f);
            }
        }

        private void OnDisable()
        {
            _disposable.Clear();
            _disposable.Dispose();
        }
    }
}
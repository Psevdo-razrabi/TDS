using Game.Player.AnimatorScripts;
using Game.Player.Interfaces;
using Game.Player.PlayerStateMashine;
using Game.Player.States.StateHandle;
using Input;
using TMPro;
using UniRx;
using UnityEngine;
using Zenject;

namespace Game.Player
{
    [RequireComponent(typeof(CharacterController))]
    public class Player : MonoBehaviour, IStateDataWorker
    {
        public ReactiveProperty<int> Text1 { get; } = new();
        
        [field: SerializeField] public TextMeshProUGUI text { get; private set; }

        private CompositeDisposable _disposable = new();
        
        public InputSystemMovement InputSystem { get; private set; }
        public InputSystemMouse InputSystemMouse { get; private set; }
        public IPlayerAim PlayerAim { get; private set; }
        public AnimatorController AnimatorController { get; private set; }
        public CharacterController CharacterController { get; private set; }
        [Inject] public PlayerConfigs PlayerConfigs { get; private set; }
        [Inject] public StateHandleChain StateChain { get; private set; }
        [Inject] public StateMachineData StateMachineData { get; private set; }
        [Inject] public AsyncWorker.AsyncWorker AsyncWorker { get; private set; }

        private InitializationStateMachine _initializationStateMachine;

        [Inject]
        private async void Construct(IPlayerAim playerAim, InputSystemMovement inputSystemMovement, 
            InputSystemMouse inputSystemMouse, AnimatorController animatorController, 
            InitializationStateMachine stateMachine)
        {
            PlayerAim = playerAim;
            InputSystem = inputSystemMovement;
            InputSystemMouse = inputSystemMouse;
            AnimatorController = animatorController;

            CharacterController = GetComponent<CharacterController>();

            _initializationStateMachine = stateMachine;

            Text1
                .Subscribe(_ => text.text = $"Dash Count : {Text1.Value}")
                .AddTo(_disposable);

            await AsyncWorker.Await(PlayerConfigs);
            StateMachineData.DashCount = PlayerConfigs.DashConfig.NumberChargesDash;
        }

        private void Update()
        {
            if(!_initializationStateMachine.PlayerStateMachine.isUpdate) return;
            
            _initializationStateMachine.PlayerStateMachine.currentStates.OnUpdateBehaviour();
        }

        private void OnDisable()
        {
            _disposable.Clear();
            _disposable.Dispose();
        }
    }
}
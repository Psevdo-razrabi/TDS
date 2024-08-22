using Game.AsyncWorker.Interfaces;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using UI.Storage;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class InputSystemUi : InputSystemBase
    {
        private bool _isActive;
        private BoolStorage _boolStorage;
        
        public InputSystemUi(PlayerComponents playerComponents, StateMachineData data, InputObserver inputObserver,
            IAwaiter asyncWorker, PlayerConfigs playerConfigs, InputSystem inputSystemNew, BoolStorage boolStorage) 
            : base(playerComponents, data, inputObserver, asyncWorker, playerConfigs, inputSystemNew)
        {
            _boolStorage = boolStorage;
        }

        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();
            InputSystemNew.UI.HideStorage.performed += OnСallingPause;
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            InputSystemNew.UI.HideStorage.performed -= OnСallingPause;
        }

        private void OnСallingPause(InputAction.CallbackContext obj)
        {
            _isActive = !_isActive;
            _boolStorage.ChangeBoolValue(_isActive);
        }
        
    }
}
using System;
using Game.AsyncWorker.Interfaces;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Input.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputSystemMouse : InputSystemBase, IMouse
    {
        public Vector2ReactiveProperty PositionMouse { get; } = new();

        private readonly CompositeDisposable _compositeDisposable = new();
        private IDisposable _disposableRightClickMouseUp;
        private IDisposable _disposableRightClickMouseDown;
        
        public InputSystemMouse(PlayerComponents playerComponents, StateMachineData data, InputObserver inputObserver, 
            IAwaiter asyncWorker, PlayerConfigs playerConfigs, InputSystem inputSystemNew) 
            : base(playerComponents, data, inputObserver, asyncWorker, playerConfigs, inputSystemNew)
        {
        }

        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();
            InputSystemNew.Mouse.MousePosition.performed += MousePosition;
            SubscribeRightMouse();
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            InputSystemNew.Mouse.MousePosition.performed -= MousePosition;
            UnsubscribeRightMouse();
        }

        private async void SubscribeRightMouse()
        {
            await AsyncWorker.AwaitLoadOrInitializeParameter(PlayerConfigs);
            
            _disposableRightClickMouseDown = InputObserver
                .SubscribeMouseRightDown()
                .Subscribe(OnClickRightDown)
                .AddTo(_compositeDisposable);
            
            _disposableRightClickMouseUp = InputObserver
                .SubscribeMouseRightUp()
                .Subscribe(OnClickRightUp)
                .AddTo(_compositeDisposable);
        }

        private void UnsubscribeRightMouse()
        {
            _disposableRightClickMouseDown.Dispose();
            _disposableRightClickMouseUp.Dispose();
            _compositeDisposable.Dispose();
            _compositeDisposable.Clear();
        }

        private void OnClickRightDown(Unit _)
        {
            Data.GetValue<ReactiveProperty<bool>>(Name.IsAiming).Value = true;
            Data.GetValue<ReactiveProperty<bool>>(Name.IsAim).Value = true;
            var config = PlayerConfigs.AnyPlayerConfigs.FowConfig;
            PlayerComponents.RadiusChanger.ChangerRadius(config.StartValueRadius, config.TimeToMaxRadius);
        }
        
        private void OnClickRightUp(Unit _)
        {
            Data.GetValue<ReactiveProperty<bool>>(Name.IsAiming).Value = false;
            Data.GetValue<ReactiveProperty<bool>>(Name.IsAim).Value = false;
            var config = PlayerConfigs.AnyPlayerConfigs.FowConfig;
            PlayerComponents.RadiusChanger.ChangerRadius(config.EndValueRadius, config.TimeToMaxRadius);
        }
        
        private void MousePosition(InputAction.CallbackContext obj)
        {
            PositionMouse.Value = obj.ReadValue<Vector2>();
        }
    }
}
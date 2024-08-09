using System;
using System.Collections.Generic;
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
        
        private void SubscribeRightMouse()
        {
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
            if(Player.PlayerConfigs.IsLoadAllConfig == false) return;
            Data.IsAiming.Value = true;
            Data.IsAim.Value = true;
            var config = Player.PlayerConfigs.FowConfig;
            Player.RadiusChanger.ChangerRadius(config.StartValueRadius, config.TimeToMaxRadius);
        }
        
        private void OnClickRightUp(Unit _)
        {
            if(Player.PlayerConfigs.IsLoadAllConfig == false) return;
            Data.IsAiming.Value = false;
            Data.IsAim.Value = false;
            var config = Player.PlayerConfigs.FowConfig;
            Player.RadiusChanger.ChangerRadius(config.EndValueRadius, config.TimeToMaxRadius);
        }
        
        private void MousePosition(InputAction.CallbackContext obj)
        {
            PositionMouse.Value = obj.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            InputSystemNew.Mouse.MousePosition.performed += MousePosition;
            SubscribeRightMouse();
        }

        private void OnDisable()
        {
            InputSystemNew.Mouse.MousePosition.performed -= MousePosition;
            UnsubscribeRightMouse();
        }
    }
}
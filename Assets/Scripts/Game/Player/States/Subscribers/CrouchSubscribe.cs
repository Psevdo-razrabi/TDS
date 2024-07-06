using System;
using Game.Player.PlayerStateMashine;
using Input;
using UniRx;
using Zenject;

namespace Game.Player.States.Subscribers
{
    public class CrouchSubscribe : IInitializable, IDisposable
    {
        private readonly InputObserver _inputObserver;
        private readonly StateMachineData _data;
        private readonly CompositeDisposable _compositeDisposable = new();
        private IDisposable _crouchButtonDown;
        private IDisposable _crouchButtonUp;

        public CrouchSubscribe(InputObserver inputObserver, StateMachineData data)
        {
            _inputObserver = inputObserver;
            _data = data;
        }

        private void SubscribeCrouch()
        {
            _crouchButtonDown = _inputObserver
                .SubscribeButtonDownCrouch()
                .Subscribe(OnStartCrouch)
                .AddTo(_compositeDisposable);
            
            _crouchButtonUp = _inputObserver
                .SubscribeButtonUpCrouch()
                .Subscribe(OnStopCrouch)
                .AddTo(_compositeDisposable);
        }

        private void UnsubscribeCrouch()
        {
            _crouchButtonDown.Dispose();
            _crouchButtonUp.Dispose();
            _compositeDisposable.Dispose();
            _compositeDisposable.Clear();
        }
        private void OnStartCrouch(Unit _) => _data.IsCrouch = true;

        private void OnStopCrouch(Unit _) => _data.IsCrouch = false;
        
        public void Initialize()
        {
            SubscribeCrouch();
        }

        public void Dispose()
        {
            UnsubscribeCrouch();
        }
    }
}
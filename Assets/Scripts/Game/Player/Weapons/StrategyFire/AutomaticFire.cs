using System;
using Cysharp.Threading.Tasks;
using Game.Player.Weapons.InterfaceWeapon;
using Input;
using UniRx;
using UnityEngine;

namespace Game.Player.Weapons.StrategyFire
{
    public class AutomaticFire : FireStrategy
    {
        private float _fireRate = 0.2f; // в конфиг
        private DateTimeOffset _lastFired;
        private CompositeDisposable _compositeDisposable = new();
        private FireComponent _fireComponent;
        private IDisposable _mouseDown;
        private IDisposable _mouseUp;
        
        public AutomaticFire(InputSystemWeapon inputSystemWeapon, MouseInputObserver mouseInputObserver) : base(inputSystemWeapon, mouseInputObserver)
        {
            //RemoveActions();
            //Subscriptions.Add(this);
            AddActionsCallbacks();
        }
        
        public override void Fire(FireComponent component)
        {
            _fireComponent = component;
        }

        protected override void AddActionsCallbacks()
        {
            _mouseUp = MouseInputObserver
                .SubscribeMouseUp()
                .Subscribe(OnMouseLeftClickUp)
                .AddTo(_compositeDisposable);
            _mouseDown = MouseInputObserver
                .SubscribeMouseDown()
                .Subscribe(OnMouseLeftClickDown)
                .AddTo(_compositeDisposable);
        }

        private void OnMouseLeftClickUp(Unit _)
        {
            Debug.LogWarning("ну епта я все подписал брат");
            SubscribeUpdate();
        }

        private void OnMouseLeftClickDown(Unit _)
        {
            Debug.LogWarning("ну епта я все отписал брат");
            RemoveActionCallbacks();
            _compositeDisposable = new CompositeDisposable();
            AddActionsCallbacks();
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            _mouseUp.Dispose();
            _mouseDown.Dispose();
            _compositeDisposable.Dispose();
            _compositeDisposable.Clear();
        }

        private void SubscribeUpdate()
        {
            Observable
                .EveryUpdate()
                .Timestamp()
                .Where(x => x.Timestamp > _lastFired.AddSeconds(_fireRate))
                .Subscribe(x =>
                {
                    _fireComponent.FireBullet();
                    _lastFired = x.Timestamp;
                })
                .AddTo(_compositeDisposable);
        }
    }
}
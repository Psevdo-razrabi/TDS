using System;
using Game.Player.Weapons.InterfaceWeapon;
using UniRx;

namespace Game.Player.Weapons.StrategyFire
{
    public class AutomaticFire : FireStrategy
    {
        private DateTimeOffset _lastFired;
        private CompositeDisposable _compositeDisposable = new();
        private IDisposable _mouseDown;
        private IDisposable _mouseUp;
        
        public AutomaticFire(FireComponent fireComponent) : base(fireComponent)
        {
            FireComponent.ActionsCleaner.RemoveAction();
            FireComponent.ActionsCleaner.AddAction(this);
            AddActionsCallbacks();
        }
        
        public override void Fire(FireComponent component)
        {
            FireComponent = component;
        }

        protected override void AddActionsCallbacks()
        {
            _mouseUp = FireComponent.MouseInputObserver
                .SubscribeMouseUp()
                .Subscribe(OnMouseLeftClickUp)
                .AddTo(_compositeDisposable);
            _mouseDown = FireComponent.MouseInputObserver
                .SubscribeMouseDown()
                .Subscribe(OnMouseLeftClickDown)
                .AddTo(_compositeDisposable);
        }

        private void OnMouseLeftClickUp(Unit _)
        {
            SubscribeUpdate();
        }

        private void OnMouseLeftClickDown(Unit _)
        {
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
                .Where(x => x.Timestamp > _lastFired.AddSeconds(FireComponent.CurrentWeapon.CurrentWeaponConfig.TimeBetweenShoots))
                .Subscribe(x =>
                {
                    FireComponent.FireBullet();
                    _lastFired = x.Timestamp;
                })
                .AddTo(_compositeDisposable);
        }
    }
}
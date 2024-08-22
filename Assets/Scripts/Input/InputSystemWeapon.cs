using System;
using Game.AsyncWorker.Interfaces;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Game.Player.Weapons;
using Game.Player.Weapons.WeaponClass;
using Input.Interface;
using UniRx;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class InputSystemWeapon : InputSystemBase, IChangeWeapon
    {
        private Subject<Unit> _delayedClickChangeMode = new();
        private Subject<Unit> _delayedClickShoot = new();
        private ChangeModeFire _changeModeFire;
        public WeaponComponent WeaponComponent { get; private set; }
        
        public InputSystemWeapon(PlayerComponents playerComponents, StateMachineData data, InputObserver inputObserver,
            IAwaiter asyncWorker, PlayerConfigs playerConfigs, InputSystem inputSystemNew, ChangeModeFire changeModeFire, DiContainer container) 
            : base(playerComponents, data, inputObserver, asyncWorker, playerConfigs, inputSystemNew)
        {
            _changeModeFire = changeModeFire;
            WeaponComponent = container.Resolve<Pistol>();
        }
        
        public void ChangeWeapon(WeaponComponent weaponComponent)
        {
            WeaponComponent = weaponComponent;
        }

        protected override void AddActionsCallbacks()
        {
            base.AddActionsCallbacks();
            InputSystemNew.Weapon.ChangeFireMode.performed += _ => _delayedClickChangeMode.OnNext(Unit.Default);
            InputSystemNew.Mouse.Shoot.performed += _ => _delayedClickShoot.OnNext(Unit.Default);
            InputSystemNew.Weapon.Reload.performed += WeaponReload;

            _delayedClickChangeMode
                .ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(async _ => await _changeModeFire.ChangeMode())  //условно задержка без переменной
                .AddTo(CompositeDisposable);
            
            _delayedClickShoot
                .ThrottleFirst(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ => WeaponComponent.FireComponent.Fire()) //задержка между выстрелами
                .AddTo(CompositeDisposable);
        }

        protected override void RemoveActionCallbacks()
        {
            base.RemoveActionCallbacks();
            InputSystemNew.Mouse.Shoot.performed -= _ => _delayedClickShoot.OnNext(Unit.Default);
            InputSystemNew.Weapon.Reload.performed -= WeaponReload;
            InputSystemNew.Disable();
            InputSystemNew.Dispose();
            CompositeDisposable.Clear();
            CompositeDisposable.Dispose();
        }

        private void WeaponReload(InputAction.CallbackContext obj) => WeaponComponent.ReloadComponent.Reload();
    }
}
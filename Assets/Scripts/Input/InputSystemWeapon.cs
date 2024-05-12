using System;
using Game.Player.Weapons;
using Game.Player.Weapons.ChangeWeapon;
using Game.Player.Weapons.WeaponClass;
using Input.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.Serialization;
using Zenject;

namespace Input
{
    public class InputSystemWeapon : InputSystemBase, IChangeWeapon
    {
        [field: SerializeField] private WeaponComponent weaponComponent;
        private Subject<Unit> _delayedClickChangeMode = new();
        private Subject<Unit> _delayedClickShoot = new();
        private ChangeModeFire _changeModeFire;
        private WeaponChange _weaponChange;

        public void ChangeWeapon(WeaponComponent weaponComponent)
        {
            this.weaponComponent = weaponComponent;
        }

        [Inject]
        private void Construct(ChangeModeFire changeModeFire, WeaponChange weaponChange)
        {
            _changeModeFire = changeModeFire;
            _weaponChange = weaponChange;
        }

        private void OnEnable()
        {
            InputSystemNew.Weapon.ChangeFireMode.performed += _ => _delayedClickChangeMode.OnNext(Unit.Default);
            InputSystemNew.Mouse.Shoot.performed += _ => _delayedClickShoot.OnNext(Unit.Default);
            InputSystemNew.Weapon.Reload.performed += WeaponReload;
            InputSystemNew.Weapon.ChangeWeapon.performed += WeaponChange;

            _delayedClickChangeMode
                .ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(async _ => await _changeModeFire.ChangeMode())  //условно задержка без переменной
                .AddTo(CompositeDisposable);
            
            
            _delayedClickShoot
                .ThrottleFirst(TimeSpan.FromSeconds(0.1f))
                .Subscribe(_ => weaponComponent.fireComponent.Fire()) //задержка между выстрелами
                .AddTo(CompositeDisposable);
        }

        private void WeaponChange(InputAction.CallbackContext obj)
        {
            _weaponChange.Change();
        }

        private void WeaponReload(InputAction.CallbackContext obj)
        {
            weaponComponent.reloadComponent.Reload();
        }
        
        private void OnDisable()
        {
            InputSystemNew.Mouse.Shoot.performed -= _ => _delayedClickShoot.OnNext(Unit.Default);
            InputSystemNew.Weapon.Reload.performed -= WeaponReload;
            InputSystemNew.Weapon.ChangeWeapon.performed -= WeaponChange;
            InputSystemNew.Disable();
            InputSystemNew.Dispose();
            CompositeDisposable.Clear();
            CompositeDisposable.Dispose();
        }
    }
}
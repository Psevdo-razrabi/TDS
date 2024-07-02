using System;
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
        
        public void ChangeWeapon(WeaponComponent weaponComponent)
        {
            WeaponComponent = weaponComponent;
        }

        [Inject]
        private void Construct(ChangeModeFire changeModeFire, DiContainer container) //временное использование
        {
            _changeModeFire = changeModeFire;
            WeaponComponent = container.Resolve<Pistol>();
        }

        private void OnEnable()
        {
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

        private void WeaponReload(InputAction.CallbackContext obj) => WeaponComponent.ReloadComponent.Reload();
        
        private void OnDisable()
        {
            InputSystemNew.Mouse.Shoot.performed -= _ => _delayedClickShoot.OnNext(Unit.Default);
            InputSystemNew.Weapon.Reload.performed -= WeaponReload;
            InputSystemNew.Disable();
            InputSystemNew.Dispose();
            CompositeDisposable.Clear();
            CompositeDisposable.Dispose();
        }
    }
}
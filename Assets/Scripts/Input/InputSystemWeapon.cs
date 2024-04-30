using System;
using Game.Player.Weapons;
using Input.Interface;
using UniRx;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class InputSystemWeapon : InputSystemBase, IChangeWeapon
    {
        private Subject<Unit> _delayedСlick = new();
        private ChangeModeFire _changeModeFire;
        private Action _mouseLeftClickUpHandler;
        private Action _mouseLeftClickDownHandler;
        private WeaponComponent _weaponComponent;

        public void ChangeWeapon(WeaponComponent weaponComponent)
        {
            _weaponComponent = weaponComponent;
        }
        
        public void OnSubscribeMouseLeftClickUp(Action action)
        {
            _mouseLeftClickUpHandler = action;
            InputSystemNew.Mouse.Shoot.performed += MouseLeftClickUpHandler;
        }

        public void OnSubscribeMouseLeftClickDown(Action action)
        {
            _mouseLeftClickDownHandler = action;
            InputSystemNew.Mouse.Shoot.canceled += MouseLeftClickDownHandler;
        }

        public void OnUnsubscribeMouseLeftClickUp()
        {
            InputSystemNew.Mouse.Shoot.performed -= MouseLeftClickUpHandler;
            _mouseLeftClickUpHandler = null;
        }

        public void OnUnsubscribeMouseLeftClickDown()
        {
            InputSystemNew.Mouse.Shoot.canceled -= MouseLeftClickDownHandler;
            _mouseLeftClickDownHandler = null;
        }

        [Inject]
        private void Construct(ChangeModeFire changeModeFire)
        {
            _changeModeFire = changeModeFire;
        }
        
        private void OnEnable()
        {
            InputSystemNew.Weapon.ChangeFireMode.performed +=_ => _delayedСlick.OnNext(Unit.Default);
            InputSystemNew.Mouse.Shoot.performed += WeaponShoot;

            _delayedСlick.ThrottleFirst(TimeSpan.FromSeconds(0.6f))
                .Subscribe(async _ => await _changeModeFire.ChangeMode())
                .AddTo(CompositeDisposable);   //условно задержка без переменной
        }

        private void WeaponShoot(InputAction.CallbackContext obj)
        {
            _weaponComponent.Fire();
        }

        private void MouseLeftClickUpHandler(InputAction.CallbackContext context) => _mouseLeftClickUpHandler?.Invoke();

        private void MouseLeftClickDownHandler(InputAction.CallbackContext context) => _mouseLeftClickDownHandler?.Invoke();
        
        private void OnDisable()
        {
            InputSystemNew.Mouse.Shoot.performed -= WeaponShoot;
            InputSystemNew.Disable();
            InputSystemNew.Dispose();
            CompositeDisposable.Clear();
            CompositeDisposable.Dispose();
        } 
    }
}
using System;
using Game.Player.States.Buffer;
using Game.Player.States.StateHandle;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class InputBuffer : IInitializable, IDisposable
    {
        private InputSystemMovement _inputSystemMovement;
        private InputSystemWeapon _inputSystemWeapon;
        private InputSystem _inputSystemNew;
        private BufferAction _bufferAction;
        private CompositeDisposable _compositeDisposable = new();
        private StateHandleChain _stateHandleChain;

        private readonly Subject<Unit> _dashClick = new();
        private readonly Subject<Unit> _weaponShotClick = new();
        private readonly Subject<Unit> _weaponReloadClick = new();

        public InputBuffer(InputSystemMovement inputSystemMovement, InputSystemWeapon inputSystemWeapon, InputSystem inputSystemNew, 
            BufferAction bufferAction, StateHandleChain stateHandleChain)
        {
            _inputSystemMovement = inputSystemMovement;
            _inputSystemWeapon = inputSystemWeapon;
            _inputSystemNew = inputSystemNew;
            _bufferAction = bufferAction;
            _stateHandleChain = stateHandleChain;
        }

        private void SubscribeActionBuffer(Subject<Unit> subject, Action inputAction)
        {
            subject
                .Where(x => _stateHandleChain.CanHandleState<PlayerDashHandle>())
                .Subscribe(_ =>
                {
                    _bufferAction.lastStateAction = inputAction;
                    _bufferAction.IsBufferAlready.Value = true;
                })
                .AddTo(_compositeDisposable);
        }

        private void RegisterActionsBuffer()
        {
            SubscribeActionBuffer(_dashClick, _inputSystemMovement.OnDash);
            SubscribeActionBuffer(_weaponShotClick, _inputSystemWeapon.WeaponComponent.FireComponent.Fire);
            SubscribeActionBuffer(_weaponReloadClick, _inputSystemWeapon.WeaponComponent.ReloadComponent.Reload);
        }

        private void DashSubject(InputAction.CallbackContext obj) => _dashClick.OnNext(Unit.Default);
        private void WeaponShotSubject(InputAction.CallbackContext obj) => _weaponShotClick.OnNext(Unit.Default);
        private void WeaponReloadSubject(InputAction.CallbackContext obj) => _weaponReloadClick.OnNext(Unit.Default);

        public void Dispose()
        {
            _inputSystemNew.Movement.Dash.performed -= DashSubject;
            _inputSystemNew.Weapon.Fire.performed -= WeaponShotSubject;
            _inputSystemNew.Weapon.Reload.performed -= WeaponReloadSubject;
            
            _compositeDisposable.Dispose();
            _compositeDisposable.Clear();
        }

        public void Initialize()
        {
            RegisterActionsBuffer();
            _inputSystemNew.Movement.Dash.performed += DashSubject;
            _inputSystemNew.Weapon.Fire.performed += WeaponShotSubject;
            _inputSystemNew.Weapon.Reload.performed += WeaponReloadSubject;
        }
    }
}
using System;
using Game.Player.States.Buffer;
using Game.Player.States.StateHandle;
using UnityEngine;
using UniRx;
using UnityEngine.InputSystem;
using Zenject;

namespace Input
{
    public class InputBuffer : MonoBehaviour
    {
        private InputSystemMouse _inputSystemMouse;
        private InputSystemMovement _inputSystemMovement;
        private InputSystemWeapon _inputSystemWeapon;
        private InputSystem _inputSystemNew;
        private BufferAction _bufferAction;
        private CompositeDisposable _compositeDisposable = new();
        private StateHandleChain _stateHandleChain;

        private readonly Subject<Unit> _dashClick = new();
        private readonly Subject<Unit> _weaponShotClick = new();
        private readonly Subject<Unit> _weaponReloadClick = new();

        [Inject]
        private void Construct(InputSystemWeapon inputSystemWeapon, InputSystemMovement inputSystemMovement, 
            InputSystemMouse inputSystemMouse, InputSystem inputSystemNew, StateHandleChain stateHandleChain, BufferAction bufferAction)
        {
            _inputSystemWeapon = inputSystemWeapon;
            _inputSystemMovement = inputSystemMovement;
            _inputSystemMouse = inputSystemMouse;
            _inputSystemNew = inputSystemNew;
            _stateHandleChain = stateHandleChain;
            _bufferAction = bufferAction;
        }

        private void SubscribeActionBuffer(Subject<Unit> subject, Action inputAction)
        {
            subject
                .Where(x => _stateHandleChain.CanHandleState<PlayerDashHandle>())
                .Subscribe(_ =>
                {
                    _bufferAction.lastStateAction = inputAction;
                    _bufferAction.IsBufferAlready.Value = true;
                    Debug.LogWarning("сколько раз я вызвался2?");
                })
                .AddTo(_compositeDisposable);
        }

        private void RegisterActionsBuffer()
        {
            SubscribeActionBuffer(_dashClick, _inputSystemMovement.OnDash);
            SubscribeActionBuffer(_weaponShotClick, _inputSystemWeapon.WeaponComponent.fireComponent.Fire);
            SubscribeActionBuffer(_weaponReloadClick, _inputSystemWeapon.WeaponComponent.reloadComponent.Reload);
        }
        
        private void OnEnable()
        {
            RegisterActionsBuffer();
            _inputSystemNew.Movement.Dash.performed += DashSubject;
            _inputSystemNew.Weapon.Fire.performed += WeaponShotSubject;
            _inputSystemNew.Weapon.Reload.performed += WeaponReloadSubject;
        }

        private void OnDisable()
        {
            _inputSystemNew.Movement.Dash.performed -= DashSubject;
            _inputSystemNew.Weapon.Fire.performed -= WeaponShotSubject;
            _inputSystemNew.Weapon.Reload.performed -= WeaponReloadSubject;
            
            _compositeDisposable.Dispose();
            _compositeDisposable.Clear();
        }

        private void DashSubject(InputAction.CallbackContext obj) => _dashClick.OnNext(Unit.Default);
        private void WeaponShotSubject(InputAction.CallbackContext obj) => _weaponShotClick.OnNext(Unit.Default);
        private void WeaponReloadSubject(InputAction.CallbackContext obj) => _weaponReloadClick.OnNext(Unit.Default);
    }
}
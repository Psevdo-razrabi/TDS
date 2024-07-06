using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Input
{
    public class InputObserver
    {
        private InputSystem _inputSystem;
        private bool _isMouseButtonDown = true;
        private bool _isButtonCrouchDown = true;

        public InputObserver(InputSystem inputSystem)
        {
            _inputSystem = inputSystem ?? throw new ArgumentNullException();
        }

        public IObservable<Unit> SubscribeMouseUp()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnMouseButtonUp(InputAction.CallbackContext ctx)
                {
                    if (_isMouseButtonDown)
                    {
                        _isMouseButtonDown = false;
                        observer.OnNext(Unit.Default);
                        Debug.LogWarning("поднял мышь");
                    }
                }

                _inputSystem.Mouse.Shoot.performed += OnMouseButtonUp;

                return Disposable.Create(() => _inputSystem.Mouse.Shoot.performed -= OnMouseButtonUp);
            });
        }
        
        public IObservable<Unit> SubscribeMouseDown()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnMouseButtonDown(InputAction.CallbackContext ctx)
                {
                    if (_isMouseButtonDown == false)
                    {
                        _isMouseButtonDown = true;
                        observer.OnNext(Unit.Default);
                        Debug.LogWarning("опустил мышь");
                    }
                }

                _inputSystem.Mouse.Shoot.canceled += OnMouseButtonDown;

                return Disposable.Create(() =>
                {
                    _inputSystem.Mouse.Shoot.canceled -= OnMouseButtonDown;
                });
            });
        }

        public IObservable<Unit> SubscribeButtonDownCrouch()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnButtonDown(InputAction.CallbackContext ctx)
                {
                    if (_isButtonCrouchDown)
                    {
                        _isButtonCrouchDown = false;
                        observer.OnNext(Unit.Default);
                        Debug.LogWarning("сел");
                    }
                }

                _inputSystem.Movement.Crouching.performed += OnButtonDown;

                return Disposable.Create(() => _inputSystem.Movement.Crouching.performed -= OnButtonDown);
            });
        }
        
        public IObservable<Unit> SubscribeButtonUpCrouch()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnButtonUp(InputAction.CallbackContext ctx)
                {
                    if (_isButtonCrouchDown == false)
                    {
                        _isButtonCrouchDown = true;
                        observer.OnNext(Unit.Default);
                        Debug.LogWarning("встал");
                    }
                }

                _inputSystem.Movement.Crouching.canceled += OnButtonUp;

                return Disposable.Create(() => _inputSystem.Movement.Crouching.canceled -= OnButtonUp);
            });
        }
    }
}
using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;


namespace Input
{
    public class InputObserver
    {
        private InputSystem _inputSystem;
        private bool _isMouseLeftButton = true;
        private bool _isMouseRightButton = true;
        private bool _isButtonCrouchDown = true;

        public InputObserver(InputSystem inputSystem)
        {
            _inputSystem = inputSystem ?? throw new ArgumentNullException();
        }

        public IObservable<Unit> SubscribeMouseLeftUp()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnMouseButtonUp(InputAction.CallbackContext ctx)
                {
                    if (_isMouseLeftButton)
                    {
                        _isMouseLeftButton = false;
                        observer.OnNext(Unit.Default);
                    }
                }

                _inputSystem.Mouse.Shoot.performed += OnMouseButtonUp;

                return Disposable.Create(() => _inputSystem.Mouse.Shoot.performed -= OnMouseButtonUp);
            });
        }
        
        public IObservable<Unit> SubscribeMouseLeftDown()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnMouseButtonDown(InputAction.CallbackContext ctx)
                {
                    if (_isMouseLeftButton == false)
                    {
                        _isMouseLeftButton = true;
                        observer.OnNext(Unit.Default);
                    }
                }

                _inputSystem.Mouse.Shoot.canceled += OnMouseButtonDown;

                return Disposable.Create(() =>
                {
                    _inputSystem.Mouse.Shoot.canceled -= OnMouseButtonDown;
                });
            });
        }
        
        public IObservable<Unit> SubscribeMouseRightDown()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnMouseButtonUp(InputAction.CallbackContext ctx)
                {
                    if (_isMouseRightButton)
                    {
                        _isMouseRightButton = false;
                        observer.OnNext(Unit.Default);
                    }
                }

                _inputSystem.Mouse.Aim.performed += OnMouseButtonUp;

                return Disposable.Create(() => _inputSystem.Mouse.Shoot.performed -= OnMouseButtonUp);
            });
        }
        
        public IObservable<Unit> SubscribeMouseRightUp()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnMouseButtonDown(InputAction.CallbackContext ctx)
                {
                    if (_isMouseRightButton == false)
                    {
                        _isMouseRightButton = true;
                        observer.OnNext(Unit.Default);
                    }
                }

                _inputSystem.Mouse.Aim.canceled += OnMouseButtonDown;

                return Disposable.Create(() =>
                {
                    _inputSystem.Mouse.Aim.canceled -= OnMouseButtonDown;
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
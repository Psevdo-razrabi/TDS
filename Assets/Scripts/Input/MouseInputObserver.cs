using System;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class MouseInputObserver
    {
        private InputSystem _inputSystem;
        private bool _isMouseButtonDown = true;

        public MouseInputObserver(InputSystem inputSystem)
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

                return Disposable.Create(() =>
                {
                    _inputSystem.Mouse.Shoot.performed -= OnMouseButtonUp;
                });
            });
        }

        public IObservable<Unit> SubscribeMouseDown()
        {
            return Observable.Create<Unit>(observer =>
            {
                void OnMouseButtonDown(InputAction.CallbackContext ctx)
                {
                    if (!_isMouseButtonDown)
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
    }
}
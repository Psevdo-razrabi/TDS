using System;
using System.Collections.Generic;
using Input.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

namespace Input
{
    public class InputSystemMouse : InputSystemBase, IMouse
    {
        public Vector2ReactiveProperty PositionMouse { get; } = new();
        private Action _mouseRightClickUpHandler;
        private Action _mouseRightClickDownHandler;
        
        public event Action RightMouseButtonDown = delegate { };
        public event Action RightMouseButtonUp = delegate { };
        
        public void OnSubscribeRightMouseClickUp(Action action)
        {
            _mouseRightClickUpHandler = action;
            InputSystemNew.Mouse.Aim.performed += MouseRightClickUpHandler;
            RightMouseButtonUp();
            RightMouseButtonUp?.Invoke();
        }

        public void OnSubscribeRightMouseClickDown(Action action)
        {
            _mouseRightClickDownHandler = action;
            InputSystemNew.Mouse.Aim.canceled += MouseRightClickDownHandler;
            RightMouseButtonDown();
            RightMouseButtonDown?.Invoke();
        }

        public void OnUnsubscribeRightMouseClickUp()
        {
            InputSystemNew.Mouse.Aim.performed -= MouseRightClickUpHandler;
            _mouseRightClickUpHandler = null;
        }

        public void OnUnsubscribeRightMouseClickDown()
        {
            InputSystemNew.Mouse.Aim.canceled -= MouseRightClickDownHandler;
            _mouseRightClickDownHandler = null;
        }
        
        private void MouseRightClickUpHandler(InputAction.CallbackContext context) =>   _mouseRightClickUpHandler?.Invoke();

        private void MouseRightClickDownHandler(InputAction.CallbackContext context) => _mouseRightClickDownHandler?.Invoke();
        
        
        private void MousePosition(InputAction.CallbackContext obj)
        {
            PositionMouse.Value = obj.ReadValue<Vector2>();
        }

        private void OnEnable()
        {
            InputSystemNew.Mouse.MousePosition.performed += MousePosition;
        }

        private void OnDisable()
        {
            InputSystemNew.Mouse.MousePosition.performed -= MousePosition;
        }
    }
}
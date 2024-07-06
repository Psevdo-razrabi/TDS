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
        public Action mouseRightClickUpHandler { get; private set; }
        public Action mouseRightClickDownHandler { get; private set; }
        
        
        public void OnSubscribeRightMouseClickDown(Action action)
        {
            mouseRightClickUpHandler = action;
            InputSystemNew.Mouse.Aim.performed += MouseRightClickUpHandler;
        }

        public void OnSubscribeRightMouseClickUp(Action action)
        {
            mouseRightClickDownHandler = action;
            InputSystemNew.Mouse.Aim.canceled += MouseRightClickDownHandler;
        }

        public void OnUnsubscribeRightMouseClickDown()
        {
            InputSystemNew.Mouse.Aim.performed -= MouseRightClickUpHandler;
            mouseRightClickUpHandler = null;
        }

        public void OnUnsubscribeRightMouseClickUp()
        {
            InputSystemNew.Mouse.Aim.canceled -= MouseRightClickDownHandler;
            mouseRightClickDownHandler = null;
        }
        
        private void MouseRightClickUpHandler(InputAction.CallbackContext context) =>  mouseRightClickUpHandler?.Invoke();

        private void MouseRightClickDownHandler(InputAction.CallbackContext context) => mouseRightClickDownHandler?.Invoke();
        
        
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
using System;
using Input.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputSystemPC : MonoBehaviour, IMouse, IMove, IJump
{
    public Vector2ReactiveProperty Move { get; } = new();
    public Vector2ReactiveProperty PositionMouse { get; } = new();
    public event Action OnJump;

    private InputSystem _input;
    private Action _mouseClickUpHandler;
    private Action _mouseClickDownHandler;
    private Action _dashButton;

    public void OnSubscribeMouseClickUp(Action action)
    {
        _mouseClickUpHandler = action;
        _input.Mouse.Aim.performed += MouseClickUpHandler;
    }

    public void OnSubscribeMouseClickDown(Action action)
    {
        _mouseClickDownHandler = action;
        _input.Mouse.Aim.canceled += MouseClickDownHandler;
    }

    public void OnUnsubscribeMouseClickUp()
    {
        _input.Mouse.Aim.performed -= MouseClickUpHandler;
        _mouseClickUpHandler = null;
    }

    public void OnUnsubscribeMouseClickDown()
    {
        _input.Mouse.Aim.canceled -= MouseClickDownHandler;
        _mouseClickDownHandler = null;
    }
    
    public void OnSubscribeDash(Action action)
    {
        _dashButton = action;
        _input.Movement.Dash.performed += DashClickHandler;
    }

    public void OnUnsubscribeDash()
    {
        _input.Movement.Dash.performed -= DashClickHandler;
        _dashButton = null;
    }

    private void MouseClickUpHandler(InputAction.CallbackContext context) => _mouseClickUpHandler?.Invoke();

    private void MouseClickDownHandler(InputAction.CallbackContext context) => _mouseClickDownHandler?.Invoke();

    private void DashClickHandler(InputAction.CallbackContext context) => _dashButton?.Invoke();

    private void MousePosition(InputAction.CallbackContext obj)
    {
        PositionMouse.Value = obj.ReadValue<Vector2>();
    }
    
    [Inject]
    private void Construct(InputSystem input)
    {
        _input = input ?? throw new ArgumentNullException($"{nameof(input)} is null");
        _input.Enable();
    }
    
    private void Jump(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void Update()
    {
        Move.Value = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
    }
    
    private void OnEnable()
    {
        _input.Mouse.MousePosition.performed += MousePosition;
        _input.Movement.Jump.performed += Jump;
    }

    private void OnDisable()
    {
        _input.Disable();
        _input.Movement.Jump.performed -= Jump;
        _input.Mouse.MousePosition.performed -= MousePosition;
    }
}

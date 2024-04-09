using System;
using Input.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputSystemPC : MonoBehaviour, IMouse, IMove, IJump
{
    public Vector2ReactiveProperty Move { get; private set; } = new();
    public Vector2ReactiveProperty PositionMouse { get; private set; } = new();
    public event Action OnJump;
    
    private InputSystem _input;

    [Inject]
    public void Construct(InputSystem input)
    {
        _input = input ?? throw new ArgumentNullException($"{nameof(input)} is null");
        _input.Enable();
    }
    public void OnEnable()
    {
        _input.Mouse.MousePosition.performed += MousePosition;
        _input.Movement.Jump.performed += Jump;
    }

    public void OnDisable()
    {
        _input.Disable();
        _input.Movement.Jump.performed -= Jump;
        _input.Mouse.MousePosition.performed -= MousePosition;
    }

    private void MousePosition(InputAction.CallbackContext obj)
    {
        PositionMouse.Value = obj.ReadValue<Vector2>();
    }
    
    private void Jump(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void Update()
    {
        Move.Value = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
    }
}

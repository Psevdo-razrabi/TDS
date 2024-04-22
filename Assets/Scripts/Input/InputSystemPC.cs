using System;
using Game.AsyncWorker;
using Game.Player.PlayerStateMashine;
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
    private Subject<Unit> _dashClick = new();
    private CompositeDisposable _compositeDisposable = new();
    private PlayerConfigs _playerConfigs;
    private AsyncWorker _asyncWorker;

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
    }

    public void OnUnsubscribeDash()
    {
        _dashButton = null;
    }
    
    private void MouseClickUpHandler(InputAction.CallbackContext context) => _mouseClickUpHandler?.Invoke();

    private void MouseClickDownHandler(InputAction.CallbackContext context) => _mouseClickDownHandler?.Invoke();

    private void DashClickHandler() => _dashButton?.Invoke();

    private void MousePosition(InputAction.CallbackContext obj)
    {
        PositionMouse.Value = obj.ReadValue<Vector2>();
    }
    
    [Inject]
    private void Construct(InputSystem input, PlayerConfigs playerConfigs, AsyncWorker worker)
    {
        _input = input ?? throw new ArgumentNullException($"{nameof(input)} is null");
        _input.Enable();
        _playerConfigs = playerConfigs;
        _asyncWorker = worker;
    }
    
    private void Jump(InputAction.CallbackContext obj)
    {
        OnJump?.Invoke();
    }

    private void Update()
    {
        Move.Value = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
    }
    
    private async void OnEnable()
    {
        _input.Mouse.MousePosition.performed += MousePosition;
        _input.Movement.Jump.performed += Jump;
        _input.Movement.Dash.performed += _ => _dashClick.OnNext(Unit.Default);

        await _asyncWorker.Await(_playerConfigs);
        
        _dashClick
            .ThrottleFirst(TimeSpan.FromSeconds(_playerConfigs.DashConfig.DelayAfterEachDash))
            .Subscribe(_ => DashClickHandler())
            .AddTo(_compositeDisposable);
    }

    private void OnDisable()
    {
        _compositeDisposable.Clear();
        _compositeDisposable.Dispose();
        _input.Movement.Jump.performed -= Jump;
        _input.Mouse.MousePosition.performed -= MousePosition;
        _input.Movement.Dash.performed -= _ => _dashClick.OnNext(Unit.Default);
        
        _input.Disable();
    }
}

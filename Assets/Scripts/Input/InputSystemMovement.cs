using System;
using Input;
using Input.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputSystemMovement : InputSystemBase, IMove, IJump
{
    public Vector2ReactiveProperty Move { get; } = new();
    public event Action OnJump;
    public Action OnDash;
    private Action _dashButton;
    private Subject<Unit> _delayedСlick = new();
    
    public void OnSubscribeDash(Action action)
    {
        _dashButton = action;
    }

    private void DashClickHandler() => _dashButton?.Invoke();
    
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
        OnDash += DashClickHandler;
        await AsyncWorker.AwaitLoadPlayerConfig(PlayerConfigs);
        Subscribe();
    }

    private void OnDisable()
    {
        OnDash -= DashClickHandler;
        Unsubscribe();
        Clear();
    }

    private void Subscribe()
    {
        InputSystemNew.Movement.Jump.performed += Jump;
        InputSystemNew.Movement.Dash.performed += _ => _delayedСlick.OnNext(Unit.Default);
        
        _delayedСlick
            .ThrottleFirst(TimeSpan.FromSeconds(PlayerConfigs.DashConfig.DelayAfterEachDash))
            .Subscribe(_ => DashClickHandler())
            .AddTo(CompositeDisposable);
    }

    private void Unsubscribe()
    {
        InputSystemNew.Movement.Jump.performed -= Jump;
        InputSystemNew.Movement.Dash.performed -= _ => _delayedСlick.OnNext(Unit.Default);
        _dashButton = null;
        OnDash -= DashClickHandler;
    }

    private void Clear()
    {
        CompositeDisposable.Clear();
        CompositeDisposable.Dispose();
        InputSystemNew.Disable();
    }
}

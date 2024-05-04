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
    
    private Action _dashButton;
    private Subject<Unit> _delayedСlick = new();
    
    public void OnSubscribeDash(Action action)
    {
        _dashButton = action;
    }

    public void OnUnsubscribeDash()
    {
        _dashButton = null;
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
        InputSystemNew.Movement.Jump.performed += Jump;
        InputSystemNew.Movement.Dash.performed += _ => _delayedСlick.OnNext(Unit.Default);

        await AsyncWorker.Await(PlayerConfigs);
        
        _delayedСlick
            .ThrottleFirst(TimeSpan.FromSeconds(PlayerConfigs.DashConfig.DelayAfterEachDash))
            .Subscribe(_ => DashClickHandler())
            .AddTo(CompositeDisposable);
    }

    private void OnDisable()
    {
        CompositeDisposable.Clear();
        CompositeDisposable.Dispose();
        InputSystemNew.Movement.Jump.performed -= Jump;
        InputSystemNew.Movement.Dash.performed -= _ => _delayedСlick.OnNext(Unit.Default);
        
        InputSystemNew.Disable();
    }
}

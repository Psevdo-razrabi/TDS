using System;
using Game.AsyncWorker.Interfaces;
using Game.Player.AnyScripts;
using Game.Player.PlayerStateMashine;
using Input;
using Input.Interface;
using UniRx;
using UnityEngine;
using UnityEngine.InputSystem;
using Zenject;

public class InputSystemMovement : InputSystemBase, IMove, ITickable
{
    public Vector2ReactiveProperty Move { get; } = new();
    public Vector2ReactiveProperty MoveNonInterpolated { get; } = new();
    public Action OnDash;
    public event Action OnClimb;
    private Action _dashButton;
    private Subject<Unit> _delayedСlick = new();
    private IDisposable _crouchButtonDown;
    private IDisposable _crouchButtonUp;
    private CompositeDisposable _compositeDisposable = new();
    
    public InputSystemMovement(PlayerComponents playerComponents, StateMachineData data, InputObserver inputObserver,
        IAwaiter asyncWorker, PlayerConfigs playerConfigs, InputSystem inputSystemNew) 
        : base(playerComponents, data, inputObserver, asyncWorker, playerConfigs, inputSystemNew)
    {
    }
    
    public void OnSubscribeDash(Action action)
    {
        _dashButton = action;
    }
    
    public void Tick()
    {
        Move.Value = new Vector2(UnityEngine.Input.GetAxis("Horizontal"), UnityEngine.Input.GetAxis("Vertical"));
        MoveNonInterpolated.Value = InputSystemNew.Movement.Move.ReadValue<Vector2>();
    }

    protected override async void AddActionsCallbacks()
    {
        base.AddActionsCallbacks();
        OnDash += DashClickHandler;
        await AsyncWorker.AwaitLoadConfigs(PlayerConfigs);
        Subscribe();
    }

    protected override void RemoveActionCallbacks()
    {
        base.RemoveActionCallbacks();
        OnDash -= DashClickHandler;
        Unsubscribe();
        Clear();
    }

    private void DashClickHandler() => _dashButton?.Invoke();
    
    private void SubscribeCrouch()
    {
        _crouchButtonDown = InputObserver
            .SubscribeButtonDownCrouch()
            .Subscribe(OnStartCrouch)
            .AddTo(_compositeDisposable);
            
        _crouchButtonUp = InputObserver
            .SubscribeButtonUpCrouch()
            .Subscribe(OnStopCrouch)
            .AddTo(_compositeDisposable);
    }

    private void UnsubscribeCrouch()
    {
        _crouchButtonDown.Dispose();
        _crouchButtonUp.Dispose();
        _compositeDisposable.Dispose();
        _compositeDisposable.Clear();
    }

    private void OnStartCrouch(Unit _) => Data.IsCrouch.Value = true;

    private void OnStopCrouch(Unit _) => Data.IsCrouch.Value = false;

    private void Subscribe()
    {
        SubscribeCrouch();
        InputSystemNew.Movement.Dash.performed += _ => _delayedСlick.OnNext(Unit.Default);
        InputSystemNew.Movement.Clamb.performed += Climbing;
        
        _delayedСlick
            .ThrottleFirst(TimeSpan.FromSeconds(PlayerConfigs.MovementConfigsProvider.DashConfig.DelayAfterEachDash))
            .Subscribe(_ => DashClickHandler())
            .AddTo(CompositeDisposable);
    }

    private void Climbing(InputAction.CallbackContext obj)
    {
        OnClimb?.Invoke();
    }

    private void Unsubscribe()
    {
        UnsubscribeCrouch();
        InputSystemNew.Movement.Clamb.performed -= Climbing;
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

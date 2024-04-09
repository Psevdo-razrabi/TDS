using System;
using System.Collections.Generic;
using Cysharp.Threading.Tasks;
using Game.Player;
using Game.Player.PlayerStateMashine.Interfase;
using UnityEngine;

public class StateMachine : MonoBehaviour
{
    private Dictionary<Type, IState> _states = null;
    public IState currentStates { get; private set; }
    public bool isUpdate { get; private set; }

    public StateMachine(params IState[] states)
    {
        _states = new Dictionary<Type, IState>(states.Length);
        foreach (var state in states)
        {
            _states.Add(state.GetType(), state);
        }
    }

    public void SwitchStates<TState>() where TState : IState,  new()
    {
        isUpdate = false;
        TryExitStates<TState>();
        GetNewState<TState>();
        TryEnterStates<TState>();
        isUpdate = true;
    }

    private void TryEnterStates<TState>() where TState : IState
    {
        if (currentStates is TState playerBehaviour)
        {
            playerBehaviour.OnEnter();
        }
    }

    private void TryExitStates<TState>() where TState : IState
    {
        if (currentStates is TState playerBehaviour)
        {
            playerBehaviour.OnExit();
        }
    }

    private void GetNewState<TState>() where TState : IState
    {
        var newState = GetState<TState>();
        currentStates = newState;
    }

    private async void StartUpdate(IState state)
    {
        while (isUpdate)
        {
            state.OnUpdateBehaviour();
            await UniTask.Yield();
        }
    }
    
    private void UpdateStates<TState>() where TState : IState
    {
        if (currentStates is TState playerBehaviour)
        {
            StartUpdate(playerBehaviour);
        }
    }
    
    private TState GetState<TState>() where TState: IState
    {
        return (TState)_states[typeof(TState)];
    }
}
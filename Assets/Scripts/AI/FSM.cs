using System;
using System.Collections.Generic;

public class FSM
{
    private List<FSMState> _states;
    private FSMState _currState;

    public FSM(List<FSMState> states)
    {
        _states = states;
        if (_states != null && _states.Count > 0)
        {
            foreach (FSMState state in _states)
            {
                state.SetFSM(this);
            }

            _currState = _states[0];
            _currState.Enter();
        }
    }

    public bool AddState(FSMState state)
    {
        if (state == null)
        {
            return false;
        }

        _states ??= new List<FSMState>();

        if (!_states.Contains(state))
        {
            _states.Add(state);
            state.SetFSM(this);
            return true;
        }
        return false;
    }

    public void Update()
    {
        _currState?.Update();
    }

    public void ChangeState(FSMState state)
    {
        int index = _states.IndexOf(state);
        if (index != -1)
        {
            ChangeState(index);
        }
        else
        {
            throw new ArgumentException("State not found");
        }
    }

    private void ChangeState(int index)
    {
        FSMState state = _states[index];
        if (state == _currState)
        {
            return;
        }
        _currState?.Exit();
        _currState = state;
        state.Enter();
    }

    internal void ChangeState<T>()
    {
        foreach (FSMState state in _states)
        {
            if(state.GetType() == typeof(T))
            {
                ChangeState(state);
                return;
            }
        }
    }
}

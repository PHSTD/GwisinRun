using System;
using System.Collections.Generic;
using UnityEngine;

public class StateMachine<T> where T : Enum
{
    private Dictionary<T, FsmState<T>> _states = new();
    private FsmState<T> _currentState;

    public void AddState(FsmState<T> state)
    {
        _states[state.StateType] = state;
    }

    public void ChangeState(T newState, FsmMessage msg = default)
    {
        if (_currentState != null && _currentState.StateType.Equals(newState))
            return; // 이미 같은 상태면 무시
        _currentState?.OnExit(newState);
        _currentState = _states[newState];
        _currentState.OnEnter(newState, msg);
    }

    public void Update(float deltaTime)
    {
        _currentState?.OnUpdate(deltaTime);
    }
}
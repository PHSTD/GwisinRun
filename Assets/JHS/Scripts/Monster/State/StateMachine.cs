using System;
using System.Collections.Generic;
using Unity.VisualScripting;

public class StateMachine<T> where T : Enum
{
    private Dictionary<T, FsmState<T>> _states = new();
    private FsmState<T> _currentState;
     public T CurrentState => _currentState != null ? _currentState.StateType : default;

    public void AddState(FsmState<T> state)
    {
        _states[state.StateType] = state;
    }
    public void ChangeState(T newState, FsmMessage msg = default)
    {
        var oldState = _currentState != null ? _currentState.StateType : default;
        _currentState?.OnExit(newState);
        _currentState = _states[newState];
        _currentState.OnEnter(oldState, msg);
    }

    public void Update(float deltaTime)
    {
        _currentState?.OnUpdate(deltaTime);
    }
}
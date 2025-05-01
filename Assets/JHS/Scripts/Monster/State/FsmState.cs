using System;

public struct FsmMessage
{
    public int Type;
    public FsmMessage(int type) { Type = type; }
}
public abstract class FsmState<T> where T : Enum
{
    public T StateType { get; private set; }

    protected FsmState(T stateType)
    {
        StateType = stateType;
    }

    public virtual void OnEnter(T fromState, FsmMessage msg) { }
    public virtual void OnUpdate(float deltaTime) { }
    public virtual void OnExit(T toState) { }
    public virtual void OnMessage(FsmMessage msg) { }
}
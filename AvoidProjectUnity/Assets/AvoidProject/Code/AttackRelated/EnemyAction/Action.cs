using UnityEngine;

public abstract class Action
{
    protected bool m_isComplete;

    public abstract void Execute();

    public abstract void Update();

    public abstract bool IsComplete();
}

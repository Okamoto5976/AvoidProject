using UnityEngine;

public abstract class Action
{
    protected bool m_isComplete;

    public abstract void Execute();

    public abstract bool IsComplete();
}

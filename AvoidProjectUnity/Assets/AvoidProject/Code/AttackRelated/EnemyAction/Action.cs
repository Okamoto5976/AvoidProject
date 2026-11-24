using UnityEngine;

public abstract class EnemyAction
{
    protected bool m_isComplete;

    protected AttackBase m_attackClass;

    public abstract void Execute();

    public abstract void Update();

    public abstract bool IsComplete();
}

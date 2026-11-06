using UnityEngine;

public abstract class AttackBase
{
    //public event System.Action<Vector2, float> Movement;
    protected Movement m_movement;
    protected Transform m_transform;
    protected AttackManager m_attackManager;

    public virtual void Initialize(Movement movement, Transform transform, AttackManager component)
    {
        m_movement = movement;
        m_transform = transform;
        m_attackManager = component;
    }
    public abstract void Execute();

    public abstract void Update();

    public abstract void Attack();
}

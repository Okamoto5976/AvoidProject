using UnityEngine;
using System;

public class Movement 
{
    private Rigidbody2D m_rd;
    private Transform m_transform;
    public event Func<Vector2, Vector2> m_onLimitStage;

    public Movement(
        Rigidbody2D rd,
        Transform target,
        Func<Vector2, Vector2> onLimitStage)
    {
        m_rd = rd;
        m_transform = target;
        m_onLimitStage = onLimitStage;
    }

    public void Move(Vector2 direction, float speed)
    {
        m_rd.linearVelocity = direction.normalized * speed;
    }


    public void Stop()
    {
        m_rd.linearVelocity = Vector2.zero;
    }
    
    public void LimitMovement()
    {
        m_transform.position = m_onLimitStage?.Invoke(m_transform.position) ?? m_transform.position;
    }

}

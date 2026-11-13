using UnityEngine;

public class Movement 
{
    private Rigidbody2D m_rd;
    private Transform m_transform;
    private StageManager m_stageManager;

    public Movement(Rigidbody2D rd, Transform target)
    {
        m_rd = rd;
        m_transform = target;
    }

    public void Initialize(StageManager manager)
    {
        m_stageManager = manager;
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
        if (m_stageManager == null)
        {
            return;
        }

        m_transform.position = m_stageManager.LimitMovement(m_transform.position);
    }

}

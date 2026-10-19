using UnityEngine;

public class StageManager
{
    private Vector2 m_center;
    private float m_limitRadius;

    public void Initialized(float radius)
    {
        m_limitRadius = radius;
    }

    public Vector2 LimitMovement(Vector2 pos)
    {
        Vector2 offset = pos - m_center;

        if(offset.magnitude > m_limitRadius)
        {
            pos = m_center + offset.normalized * m_limitRadius;
        }

        return pos;
    }
}

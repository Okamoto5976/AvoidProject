using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D m_rd;

    private StageManager m_stageManager;

    public void Initialized(StageManager manager)
    {
        m_stageManager = manager;
    }

    private void Awake()
    {
        m_rd = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 direction, float speed)
    {
        m_rd.linearVelocity = direction.normalized * speed;
    }


    public void Stop()
    {
        m_rd.linearVelocity = Vector2.zero;
    }
    
    private void Update()
    {
        transform.position = m_stageManager.LimitMovement(transform.position);
    }

}

using UnityEngine;

public class Movement : MonoBehaviour
{
    private Rigidbody2D m_rd;
    
    private void Awake()
    {
        m_rd = GetComponent<Rigidbody2D>();
    }

    public void Move(Vector2 value , float speed)
    {
        m_rd.linearVelocity = new Vector2(value.x * speed, value.y * speed);
    }

}

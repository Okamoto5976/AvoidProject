using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputProvider m_inputProvider;
    private Rigidbody2D m_rd;
    private float m_jumpForce = 5f;
    
    private void Awake()
    {
        m_inputProvider = new InputProvider();
        m_rd = GetComponent<Rigidbody2D>();
    }

    private void Update()
    {
        //Debug.Log($"{m_inputProvider.InputMove}");
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    { 
        m_rd.linearVelocity = new Vector2(m_inputProvider.InputMove.x *  m_jumpForce, m_inputProvider.InputMove.y * m_jumpForce);

    }
}

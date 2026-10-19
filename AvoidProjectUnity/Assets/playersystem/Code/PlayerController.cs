using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private InputProvider m_inputProvider;
    private Rigidbody2D m_rd;
    private float m_jumpForce = 5f;
    private float m_moveSpeed = 5f;
    private float m_sprintSpeed = 10f;
    private bool m_isSprinting = false;

    private Vector2 m_velocity;
    
    private void Awake()
    {
        m_inputProvider = new InputProvider();
        m_inputProvider.OnInputMove += InputMove;
        m_inputProvider.OnInputInteract += InputInteract;
        m_inputProvider.OnInputSprint += InputSprint;
        m_inputProvider.OnInputAttack += InputAttack;
        m_inputProvider.OnInputRecover += InputRecover;
        m_inputProvider.OnInputAvoid += InputAvoid;
        m_rd = GetComponent<Rigidbody2D>();
    }

    private void OnDisable()
    {
        m_inputProvider.OnInputMove -= InputMove;
        m_inputProvider.OnInputInteract -= InputInteract;
        m_inputProvider.OnInputSprint -= InputSprint;
        m_inputProvider.OnInputAttack -= InputAttack;
        m_inputProvider.OnInputRecover -= InputRecover;
        m_inputProvider.OnInputAvoid -= InputAvoid;
        m_inputProvider.Disable();
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
        float speed = m_isSprinting ? m_sprintSpeed : m_moveSpeed;

        m_rd.linearVelocity = new Vector2(m_velocity.x *  speed, m_velocity.y * speed);

    }

    private void InputSprint(bool isSprinting)
    {
        m_isSprinting = !m_isSprinting;
    }
    private void InputMove(Vector2 value)
    {
        m_velocity = value;
    }
    private void InputInteract()
    {
        Debug.Log("インタラクト！");
    }
    private void InputAttack()
    {
        Debug.Log("攻撃！");
    }
    private void InputRecover()
    {
        Debug.Log("回復！");
    }
    private void InputAvoid()
    {
        Debug.Log("回避！");
    }
}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour
{
    //component or class
    private Rigidbody2D m_rd;
    private Movement m_movement;
    private HpController m_hpController;

    private InputProvider m_inputProvider;
    private PlayerInteract m_playerInteract;
  
    //InputRelated
    private bool m_isSprinting = false;
    private bool m_isAvoiding = false;
    private bool m_isInteracting = false;
    private bool m_isWaitingAvoid = false;
    private bool m_isRecovering = false;

    //PlayerStatus
    private float m_moveSpeed = 5f;
    private float m_sprintSpeed = 10f;
    private float m_avoidDelay = 5f / 60f; // 回避開始までのフレーム数
    private float m_avoidDuration = 0.2f;
    private float m_avoidSpeed = 15f;
    private float m_recoverMoveSpeed = 2f;

    private int m_avoidFrameCount = 0;
    
    private Vector2 m_velocity;

    private bool m_isInvincible = false;
    private bool m_hasWeapon = false;

    private void Awake()
    {
        m_playerInteract = new PlayerInteract();
        m_rd = GetComponent<Rigidbody2D>();
        m_movement = GetComponent<Movement>();
        m_hpController = GetComponent<HpController>();

        //Input Initialize
        m_inputProvider = new InputProvider();
        m_inputProvider.OnInputMove += InputMove;
        m_inputProvider.OnInputInteract += InputInteract;
        m_inputProvider.OnInputSprint += InputSprint;
        m_inputProvider.OnInputAttack += InputAttack;
        m_inputProvider.OnInputRecover += InputRecover;
        m_inputProvider.OnInputAvoid += InputAvoid;
        m_inputProvider.OnInputInteractHold += InputInteractHold;
        m_inputProvider.OnInputRecoverHold += InputRecoverHold;

        m_inputProvider.PlayerEnable();
    }

    private void OnEnable()
    {
        m_inputProvider.OnInputMove += InputMove;
        m_inputProvider.OnInputInteract += InputInteract;
        m_inputProvider.OnInputSprint += InputSprint;
        m_inputProvider.OnInputAttack += InputAttack;
        m_inputProvider.OnInputRecover += InputRecover;
        m_inputProvider.OnInputAvoid += InputAvoid;
        m_inputProvider.OnInputInteractHold += InputInteractHold;
        m_inputProvider.OnInputRecoverHold += InputRecoverHold;

        m_inputProvider.PlayerEnable();
    }

    private void OnDisable()
    {
        m_inputProvider.OnInputMove -= InputMove;
        m_inputProvider.OnInputInteract -= InputInteract;
        m_inputProvider.OnInputSprint -= InputSprint;
        m_inputProvider.OnInputAttack -= InputAttack;
        m_inputProvider.OnInputRecover -= InputRecover;
        m_inputProvider.OnInputAvoid -= InputAvoid;
        m_inputProvider.OnInputInteractHold -= InputInteractHold;
        m_inputProvider.OnInputRecoverHold -= InputRecoverHold;

        m_inputProvider.PlayerDisable();
    }

    public void Initialize()
    {
  

    }

    private void Update()
    {
        if (m_isWaitingAvoid)
        {
            m_avoidFrameCount++;

            if (m_avoidFrameCount >= 5)
            {
                m_isWaitingAvoid = false;
                StartCoroutine(AvoidCoroutine());
            }
        }
       
        

        if (m_isInteracting)
        {
            //m_playerInteract.Interact();
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (m_isAvoiding || m_isWaitingAvoid)
        {
            return;
        }

        float speed = m_moveSpeed;

        if (m_isSprinting)
        {
            speed = m_sprintSpeed;
        }

        if (m_isRecovering)
        {
            speed = m_recoverMoveSpeed;
        }
        m_movement.Move(m_velocity, speed);

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
        Debug.Log("インタラクト！",this);
    }
    private void InputAttack()
    {
        m_isRecovering = false;
        Debug.Log("攻撃！",this);
    }
    private void InputRecover()
    {
        Debug.Log("回復！",this);
    }
    private void InputAvoid()
    {
        if (m_isAvoiding || m_isWaitingAvoid)
        {
            return;
        }

        m_isRecovering = false;

        m_isWaitingAvoid = true;
        m_avoidFrameCount = 0;
    }

    public void Damage(int damage)
    {
        if (m_isInvincible)
        {
            Debug.Log("回避中のためダメージ無効！",this);
            return;
        }
        m_hpController.Damage(damage);
    }

    private void InputInteractHold(bool isHolding)
    {
        m_isInteracting = isHolding;
        if (isHolding)
        {
            m_isRecovering = false;
        }
    }
    private void InputRecoverHold(bool isHolding)
    {
        m_isRecovering = isHolding;

        if (m_isRecovering)
        {
            Debug.Log("回復開始！", this);
        }
        else
        {
            Debug.Log("回復終了！", this);
        }
    }


    private IEnumerator AvoidCoroutine()
    {
        m_isAvoiding = true;
       

        // 回避開始と同時に無敵化
        m_isInvincible = true;

        // 回避移動を開始
        m_movement.Move(m_velocity, m_avoidSpeed);

        Debug.Log("回避開始・無敵化！", this);

        // 回避時間
        yield return new WaitForSeconds(m_avoidDuration);

        // 回避終了
        m_movement.Stop();

        m_isInvincible = false;
        m_isAvoiding = false;

        Debug.Log("回避終了・無敵解除！", this);  
    }

    public void SetHasWeapon(bool value)
    {
        m_hasWeapon = value;
    }
}

using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamageable
{
    //component or class
    private Rigidbody2D m_rd;
    private Movement m_movement;
    private HpController m_hpController;

    private InputProvider m_inputProvider;
    private PlayerInteract m_playerInteract;
    private PlayerState m_playerState = PlayerState.Idle;

    //InputRelated
    private bool m_isSprinting = false;
    private bool m_isAvoiding = false;
    private bool m_isInteracting = false;
    private bool m_isWaitingAvoid = false;
    private bool m_isRecovering = false;
    private bool m_isInvincible = false;
    private bool m_hasWeapon = false;
    //PlayerStatus
    private float m_moveSpeed;
    private float m_sprintSpeed;
    private float m_avoidDuration;
    private float m_avoidSpeed;
    private float m_recoverMoveSpeed;
    private float m_interactRange;
    private float m_interactHoldTime;
    private float m_attackStateDuration;

    private int m_avoidStartFrame;
    private int m_avoidFrameCount = 0;
    private int m_justAvoidFrameCount = 0;
    private int m_pendingDamage = 0;
    private bool m_isWaitingJustAvoid = false;
    private int m_justAvoidFrame;

    private Vector2 m_velocity;


    //PlayerHpHeal
    private float m_recoverTimer = 0;
    private float m_recoverDuration;
    private int m_recoverValue;


    private int m_hasRecoverCount;


    [SerializeField] private Team m_team;
    public Team Team => m_team;


    private void Awake()
    {
        m_playerInteract = new PlayerInteract(transform);
        m_rd = GetComponent<Rigidbody2D>();
        m_movement = new Movement(m_rd, transform);
        m_hpController = GetComponent<HpController>();

        m_inputProvider = new InputProvider();
      
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
    private MeleeAttack m_meleeAttack;

    public void Initialize(PlayerData pData, RuntimeData rData, StageManager stageManager)
    {


        //Debug.Log("PlayerController Initialize 実行！");
        m_hpController.Initialize(pData.HP);

        m_hasRecoverCount = rData.PotionAmount;

        pData.m_attackData.PlayerTestSetAction();

        m_meleeAttack = new(pData.m_attackData);
        //m_meleeAttack.SetTransfrom(this.transform);

 

        m_moveSpeed = pData.Speed;  
        m_sprintSpeed = pData.SprintSpeed;
        m_attackStateDuration = pData.AttackStateDuration;

        m_avoidSpeed = pData.Speed * pData.AvoidSpeedRate;
        m_avoidStartFrame = pData.AvoidStartFrame;
        m_avoidDuration = pData.AvoidDuration;
        m_justAvoidFrame = pData.JustAvoidFrame;

        m_recoverMoveSpeed = pData.RecoverMoveSpeed;
        m_recoverDuration = pData.RecoverDuration;
        m_recoverValue = pData.RecoverValue;

        m_interactRange = pData.InteractRange;
        m_interactHoldTime = pData.InteractHoldTime;


      //m_movement.Initialize(stageManager);
        //Debug.Log("移動速度：" + m_moveSpeed);
    }

   public void SetAttackManager(AttackManager component)
    {
        //m_meleeAttack.SetAttackManager(component);

        m_meleeAttack.Initialize(m_movement, transform, component);
    }

    private void Update()
    {
        m_movement.LimitMovement();

        if (m_isWaitingAvoid)
        {
            m_avoidFrameCount++;

            if (m_avoidFrameCount >= m_avoidStartFrame)
            {
                m_isWaitingAvoid = false;
                StartCoroutine(AvoidCoroutine());
            }
        }
       
        
        if (m_isWaitingJustAvoid)
        {
            m_justAvoidFrameCount++;
            if (m_justAvoidFrameCount >= m_justAvoidFrame)
            {
                m_isWaitingJustAvoid = false;

                m_hpController.Damage(m_pendingDamage);
                m_pendingDamage = 0;
               
                //Debug.Log("回避失敗！ダメージを受けました", this);
                if (m_hpController. Hp <= 0)
                {
                    Dead();
                }
            }
        }

        if (m_isInteracting)
        {
            m_playerInteract.Interact(this, Time.deltaTime, m_interactHoldTime);
        }
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void Move()
    {
        if (m_playerState == PlayerState.Dead)
        {
            return;
        }
        if (m_isAvoiding || m_isWaitingAvoid)
        {
            return;
        }

        float speed = m_moveSpeed;

        if (m_isSprinting)
        {
            speed = m_sprintSpeed;
            ChangeState(PlayerState.Sprint);
        }

        else if (m_isRecovering)
        {
            speed = m_recoverMoveSpeed;
            ChangeState(PlayerState.Recover);
        }
        else if (m_velocity != Vector2.zero)
        {
            ChangeState(PlayerState.Move);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
        m_movement.Move(m_velocity, speed);

    }

    private void InputSprint(bool isSprinting)
    {
        if (m_playerState == PlayerState.Dead) return;
        m_isSprinting = !m_isSprinting;
    }
    private void InputMove(Vector2 value)
    {
        if (m_playerState == PlayerState.Dead) return;  
        m_velocity = value;
    }

    private void InputInteract()
    {
        if (m_playerState == PlayerState.Dead) return;
        //Debug.Log("インタラクト！",this);

    }


    private void InputAttack()
    {
        if (m_playerState == PlayerState.Dead) return;  
        m_isRecovering = false;
        ChangeState(PlayerState.Attack);
        //Debug.Log("攻撃！",this);

        m_meleeAttack.Execute();


        StartCoroutine(AttackCoroutine());


    }
    private void InputRecover()
    {
        //Debug.Log("回復！",this);
    }
    private void InputAvoid()
    {
        if (m_playerState == PlayerState.Dead) return;
        if (m_isWaitingJustAvoid)
        {
           m_isWaitingJustAvoid = false;

            m_pendingDamage = 0;
            m_isRecovering = false;

            // 通常回避の待機も解除
            m_isWaitingAvoid = false;
            m_avoidFrameCount = 0;

            //Debug.Log("ジャスト回避成功！", this);

            StartCoroutine(AvoidCoroutine());
            return;
        }

        if (m_isAvoiding || m_isWaitingAvoid)
        {
            return;
        }

        m_isRecovering = false;

        m_isWaitingAvoid = true;
        m_avoidFrameCount = 0;
    }

    public void TakeDamage(int damage)
    {
        if (m_isInvincible)
        {
            //Debug.Log("回避中のためダメージ無効！",this);
            return;
        }

        if (!m_isWaitingJustAvoid)
        {
            m_isWaitingJustAvoid = true;
            m_justAvoidFrameCount = 0;
        }
        m_pendingDamage += damage;

        //Debug.Log("攻撃Hit！ジャスト回避受付開始！", this);
    }

    private void InputInteractHold(bool isHolding)
    {
        if (m_playerState == PlayerState.Dead) return;
        m_isInteracting = isHolding;

        if (isHolding)
        {
            m_isRecovering = false;
            ChangeState(PlayerState.Interact);
            m_playerInteract.StartInteract(this, m_interactRange);
        }
        else
        {
            m_playerInteract.EndInteract();

            if (!m_isAvoiding)
            {
                ChangeState(PlayerState.Idle);
            }
        }
    }

    private void InputRecoverHold(bool isHolding)
    {
        if (m_playerState == PlayerState.Dead) return;
        m_isRecovering = isHolding;

        if (m_isRecovering)
        {
            if (m_hasRecoverCount <= 0) return;

            StartCoroutine(RecoverCoroutine());
            //Debug.Log("回復開始！", this);
        }
        else
        {
            m_recoverTimer = 0;
            //Debug.Log("回復終了！", this);
        }
    }

    private IEnumerator RecoverCoroutine()
    {
        m_recoverTimer = 0;
        while (true)
        {
            if (!m_isRecovering) yield break;

            m_recoverTimer += Time.deltaTime;
            if (m_recoverTimer >= m_recoverDuration)
            {
                m_hasRecoverCount--;
                m_hpController.Heal(m_recoverValue);
                yield break;
            }
            yield return null;
        }
    }

    private IEnumerator AvoidCoroutine()
    {
        m_isAvoiding = true;

        ChangeState(PlayerState.Avoid);

        // 回避開始と同時に無敵化
        m_isInvincible = true;

        // 回避移動を開始
        m_movement.Move(m_velocity, m_avoidSpeed);

        //Debug.Log("回避開始・無敵化！", this);

        // 回避時間
        yield return new WaitForSeconds(m_avoidDuration);

        // 回避終了
        m_movement.Stop();

        m_isInvincible = false;
        m_isAvoiding = false;

        if (m_playerState == PlayerState.Avoid)
        {
            ChangeState(PlayerState.Idle);
        }

        //Debug.Log("回避終了・無敵解除！", this);  
    }

    public void SetHasWeapon(bool value)
    {
        m_hasWeapon = value;
    }

    private void ChangeState(PlayerState newState)
    {
        if (m_playerState == newState) return;
        m_playerState = newState;
        //Debug.Log("Player State: " + m_playerState, this);
    }

    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(m_attackStateDuration);

        if (m_playerState == PlayerState.Attack)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    private void Dead()
    {
        ChangeState(PlayerState.Dead);

        m_velocity = Vector2.zero;
        m_isSprinting = false;
        m_isRecovering = false;
        m_isInteracting = false;

        m_isWaitingAvoid = false;
        m_isWaitingJustAvoid = false;
        m_isAvoiding = false;
        m_isInvincible = false;

        m_movement.Stop();

        //Debug.Log("プレイヤー死亡！", this);
    }
    [ContextMenu("テスト用/強制死亡")]
    private void TestDead()
    {
        Dead();
    }
    [ContextMenu("テスト用/ジャスト回避受付")]
    private void TestJustAvoid()
    {
        TakeDamage(10);
    }
}


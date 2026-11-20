using UnityEngine;
using System.Collections;

public class PlayerController : MonoBehaviour, IDamageable
{
    //==================================================
    // 変数宣言
    //==================================================

    #region Component / Class

    private Rigidbody2D m_rd;
    private Movement m_movement;
    private HpController m_hpController;

    private InputProvider m_inputProvider;
    private PlayerInteract m_playerInteract;
    private MeleeAttack m_meleeAttack;

    private PlayerState m_playerState = PlayerState.Idle;

    #endregion


    #region Player Flags

    private bool m_isSprinting = false;
    private bool m_isAvoiding = false;
    private bool m_isInteracting = false;
    private bool m_isWaitingAvoid = false;
    private bool m_isWaitingAvoidInput = false;
    private bool m_isRecovering = false;
    private bool m_isInvincible = false;
    private bool m_hasWeapon = false;
    private bool m_isJustAvoidSuccess = false;
    private bool m_isDamageInvincible = false;
    private bool m_isWaitingJustAvoid = false;
    // 入力待機フラグ
    private bool m_isPendingAttack = false;
    private bool m_isPendingAvoid = false;
    private bool m_pendingInteract = false;
    private bool m_pendingRecover = false;

    #endregion


    #region Player Status

    private float m_moveSpeed;
    private float m_sprintSpeed;
    private float m_avoidDuration;
    private float m_avoidSpeed;
    private float m_recoverMoveSpeed;
    private float m_interactRange;
    private float m_interactHoldTime;
    private float m_attackStateDuration;

    private float m_damageInvincibleDuration;
    private float m_damageInvicibleTimer = 0f;

    private int m_avoidStartFrame;
    private int m_avoidFrameCount = 0;
    private int m_justAvoidFrameCount = 0;
    private int m_pendingDamage = 0;
    private int m_justAvoidFrame;
    private int m_dhp;

    private Vector2 m_velocity;
    private Vector2 m_avoidDirection;
    private Vector2 m_lastMoveDirection = Vector2.zero;
    private Vector2 m_previousMoveInput = Vector2.zero;

    #endregion


    #region Recover Status

    private float m_recoverTimer = 0f;
    private float m_recoverDuration;
    private int m_recoverValue;
    private int m_hasRecoverCount;
    private Coroutine m_recoverCoroutine;

    #endregion


    #region Team

    [SerializeField] private Team m_team;
    public Team Team => m_team;

    #endregion


    //==================================================
    // 初期化処理
    //==================================================

    #region Initialize

    private void Awake()
    {
        m_playerInteract = new PlayerInteract(transform);

        m_rd = GetComponent<Rigidbody2D>();
        m_movement = new Movement(m_rd, transform);
        m_hpController = GetComponent<HpController>();
    }


    private void OnEnable()
    {
        if (m_inputProvider == null) return;

        m_inputProvider.PlayerEnable();
    }


    private void OnDisable()
    {
        if (m_inputProvider == null) return;

        m_inputProvider.PlayerDisable();
    }


    private void OnDestroy()
    {
        if (m_inputProvider == null) return;

        m_inputProvider.OnInputMove -= InputMove;
        m_inputProvider.OnInputSprint -= InputSprint;
        m_inputProvider.OnInputAttack -= InputAttack;
        m_inputProvider.OnInputAvoid -= InputAvoid;
        m_inputProvider.OnInputInteractHold -= InputInteract;
        m_inputProvider.OnInputRecover -= InputRecover;
    }


    public void Initialize(
        PlayerData pData,
        RuntimeData rData,
        StageManager stageManager,
        InputProvider inputProvider)
    {
        // InputProviderの設定
        m_inputProvider = inputProvider;

        m_inputProvider.OnInputMove += InputMove;
        m_inputProvider.OnInputSprint += InputSprint;
        m_inputProvider.OnInputAttack += InputAttack;
        m_inputProvider.OnInputAvoid += InputAvoid;
        m_inputProvider.OnInputInteractHold += InputInteract;
        m_inputProvider.OnInputRecover+= InputRecover;


        // HP・回復薬の初期化
        m_hpController.Initialize(pData.HP);
        m_hasRecoverCount = rData.PotionAmount;


        // 攻撃の初期化
        pData.m_attackData.PlayerTestSetAction();

        m_meleeAttack = new(pData.m_attackData);


        // プレイヤーのステータス設定
        m_dhp = pData.Dhp;

        m_moveSpeed = pData.Speed;
        m_sprintSpeed = pData.SprintSpeed;
        m_attackStateDuration = pData.AttackStateDuration;


        // 回避設定
        m_avoidSpeed = pData.Speed * pData.AvoidSpeedRate;
        m_avoidStartFrame = pData.AvoidStartFrame;
        m_avoidDuration = pData.AvoidDuration;
        m_justAvoidFrame = pData.JustAvoidFrame;


        // 回復設定
        m_recoverMoveSpeed = pData.RecoverMoveSpeed;
        m_recoverDuration = pData.RecoverDuration;
        m_recoverValue = pData.RecoverValue;


        // インタラクト設定
        m_interactRange = pData.InteractRange;
        m_interactHoldTime = pData.InteractHoldTime;


        // ダメージ設定
        m_damageInvincibleDuration = pData.DamageInvincibleDuration;
    }


    public void SetAttackManager(AttackManager component)
    {
        m_meleeAttack.Initialize(m_movement, transform, component);
    }

    #endregion


    //==================================================
    // 更新処理
    //==================================================

    #region Update

    private void Update()
    {
        if (m_playerState == PlayerState.Dead) return;

        ProcessInputEvents();

        // ダメージ後の無敵時間
        UpdateDamageInvincible();


        // 移動範囲制限
        m_movement.LimitMovement();


        // 回避開始待機
        UpdateAvoid();


        // ジャスト回避判定
        UpdateJustAvoid();


        // インタラクト
        if (m_isInteracting)
        {
            m_playerInteract.Interact(
                this,
                Time.deltaTime,
                m_interactHoldTime);
        }
    }
    private void ProcessInputEvents()
    {
        // 攻撃
        if (m_isPendingAttack)
        {
            m_isPendingAttack = false;
            ExecuteAttack();
        }

        // 回避
        if (m_isPendingAvoid)
        {
            m_isPendingAvoid = false;
            ExecuteAvoid();
        }

        // インタラクト
        if (m_pendingInteract)
        {
            bool isHolding = m_pendingInteract;

            ExecuteInteract(isHolding);
        }

        // 回復
        if (m_pendingRecover)
        {
            bool isHolding = m_pendingRecover;

            ExecuteRecover(isHolding);
        }
    }
 

    #region Execute Input

    private void ExecuteAttack()
    {
        if (!m_hasWeapon)
        {
            Debug.Log("武器を持っていないため攻撃できません！", this);
            return;
        }

        if (m_isInteracting)
        {
            m_isInteracting = false;
            m_playerInteract.EndInteract();
        }
        m_isRecovering = false;

        ChangeState(PlayerState.Attack);
        m_meleeAttack.Execute();
        StartCoroutine(AttackCoroutine());
    }


    private void ExecuteAvoid()
    {
        if (m_isAvoiding || m_isWaitingAvoid || m_isWaitingAvoidInput) return;
        
        if (m_isInteracting)
        {
            m_isInteracting = false;
            m_playerInteract.EndInteract();
        }

        m_isRecovering = false;

        // ジャスト回避成功
        if (m_isWaitingJustAvoid)
        {
            m_isWaitingJustAvoid = false;
            m_pendingDamage = 0;
            m_isJustAvoidSuccess = true;
        }

        // 現在の移動方向を優先
        if (m_velocity.sqrMagnitude > 0.01f)
        {
            m_avoidDirection = m_velocity.normalized;
        }
        // 停止中なら最後の移動方向を使用
        else if (m_lastMoveDirection.sqrMagnitude > 0.01f)
        {
            m_avoidDirection = m_lastMoveDirection.normalized;
        }
        else
        {
            m_isWaitingAvoidInput = true;
            return;
        }

        m_isWaitingAvoid = true;
        m_avoidFrameCount = 0;
        m_isInvincible = true;
    }


    private void ExecuteInteract(bool isHolding)
    {
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


    private void ExecuteRecover(bool isHolding)
    {
        if (isHolding)
        {
            if (m_hasRecoverCount <= 0) return;

            m_isRecovering = true;

            if (m_recoverCoroutine == null)
            {
                m_recoverCoroutine = StartCoroutine(RecoverCoroutine());
            }
        }
        else
        {
            m_isRecovering = false;
            m_recoverTimer = 0f;
        }
    }

    #endregion


    private void FixedUpdate()
    {
        Move();
    }


    private void UpdateDamageInvincible()
    {
        if (!m_isDamageInvincible) return;

        m_damageInvicibleTimer += Time.deltaTime;

        if (m_damageInvicibleTimer >= m_damageInvincibleDuration)
        {
            m_isDamageInvincible = false;
            m_damageInvicibleTimer = 0f;
        }
    }


    private void UpdateAvoid()
    {
        if (!m_isWaitingAvoid) return;

        m_avoidFrameCount++;

        if (m_avoidFrameCount >= m_avoidStartFrame)
        {
            m_isWaitingAvoid = false;
            StartCoroutine(AvoidCoroutine());
        }
    }


    private void UpdateJustAvoid()
    {
        if (!m_isWaitingJustAvoid) return;

        m_justAvoidFrameCount++;

        if (m_justAvoidFrameCount < m_justAvoidFrame) return;

        m_isWaitingJustAvoid = false;

        int damage = m_pendingDamage;
        m_pendingDamage = 0;

        if (damage <= 0) return;

        // HP減少
        m_hpController.Damage(damage);

        // HPが0になった場合
        if (m_hpController.Hp <= 0)
        {
            Dead();
            return;
        }

        // ダメージ後の無敵時間
        m_isDamageInvincible = true;
        m_damageInvicibleTimer = 0f;

        Debug.Log("回避失敗！ダメージを受けました", this);
    }

    #endregion


    //==================================================
    // 移動処理
    //==================================================

    #region Movement

    private void Move()
    {
        if (m_playerState == PlayerState.Dead) return;

        if (m_isAvoiding || m_isWaitingAvoid || m_isWaitingAvoidInput)
        {
            return;
        }

        float speed = m_moveSpeed;


        if (m_isRecovering)
        {
            speed = m_recoverMoveSpeed;
            ChangeState(PlayerState.Recover);
        }
        else if (m_isSprinting)
        {
            speed = m_sprintSpeed;
            ChangeState(PlayerState.Sprint);
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


    private void InputMove(Vector2 value)
    {
        if (m_playerState == PlayerState.Dead) return;

        m_velocity = value;

        if (value.sqrMagnitude > 0.01f)
        {
            int previousCount = 0;
            int currentCount = 0;

            if (Mathf.Abs(m_previousMoveInput.x) > 0.01f)
                previousCount++;

            if (Mathf.Abs(m_previousMoveInput.y) > 0.01f)
                previousCount++;

            if (Mathf.Abs(value.x) > 0.01f)
                currentCount++;

            if (Mathf.Abs(value.y) > 0.01f)
                currentCount++;


            // 斜め入力への変化、または同じ方向数なら保存
            if (currentCount >= previousCount)
            {
                m_lastMoveDirection = value.normalized;
            }
        }

        m_previousMoveInput = value;


        // 回避準備中に移動入力があった場合
        if (m_isWaitingAvoidInput && value.sqrMagnitude > 0.01f)
        {
            m_isWaitingAvoidInput = false;

            m_avoidDirection = value.normalized;

            m_isWaitingAvoid = true;
            m_avoidFrameCount = 0;
        }
    }

    #endregion


    //==================================================
    // 入力処理
    //==================================================

    #region Input

    private void InputSprint(bool isSprinting)
    {
        if (m_playerState == PlayerState.Dead) return;

        m_isSprinting = isSprinting;
    }

    private void InputInteract(bool isHolding)
    {
        if (m_playerState == PlayerState.Dead) return;

        m_pendingInteract = isHolding;
    }

    private void InputAttack()
    {
        if (m_playerState == PlayerState.Dead) return;

        m_isPendingAttack = true;
    }

    private void InputRecover(bool isHolding)
    {
        if (m_playerState == PlayerState.Dead) return;

        m_pendingRecover = isHolding;
    }

    private void InputAvoid()
    {
        if (m_playerState == PlayerState.Dead) return;

        m_isPendingAvoid = true;
    }

    #endregion


    //==================================================
    // ダメージ処理
    //==================================================

    #region Damage

    public void TakeDamage(int damage)
    {
        if (m_playerState == PlayerState.Dead) return;


        // 回避中はダメージ無効
        if (m_isInvincible)
        {
            Debug.Log("回避中のためダメージ無効！", this);
            return;
        }


        // ダメージ後の無敵時間中は無効
        if (m_isDamageInvincible)
        {
            return;
        }


        // ジャスト回避受付中なら追加ダメージを受け付けない
        if (m_isWaitingJustAvoid)
        {
            return;
        }


        m_isWaitingJustAvoid = true;
        m_justAvoidFrameCount = 0;

        // 最初の攻撃分だけ記録
        m_pendingDamage = damage;

        Debug.Log("攻撃Hit！ジャスト回避受付開始！", this);
    }


    public void SetIsInvincible(bool value)
    {
        m_isInvincible = value;
    }

    #endregion


    //==================================================
    // コルーチン処理
    //==================================================

    #region Coroutine

    private IEnumerator RecoverCoroutine()
    {
        m_recoverTimer = 0f;

        while (m_isRecovering &&
               m_playerState != PlayerState.Dead)
        {
            m_recoverTimer += Time.deltaTime;

            if (m_recoverTimer >= m_recoverDuration)
            {
                m_hasRecoverCount--;
                m_hpController.Heal(m_recoverValue);
                break;
            }

            yield return null;
        }

        m_recoverTimer = 0f;
        m_recoverCoroutine = null;
    }


    private IEnumerator AvoidCoroutine()
    {
        if (m_playerState == PlayerState.Dead)
            yield break;

        m_isAvoiding = true;

        ChangeState(PlayerState.Avoid);

        m_isInvincible = true;


        // ジャスト回避成功
        if (m_isJustAvoidSuccess)
        {
            Debug.Log("ジャスト回避成功！");

            m_isJustAvoidSuccess = false;
        }


        // 回避方向に移動
        m_movement.Move(m_avoidDirection, m_avoidSpeed);

        yield return new WaitForSeconds(m_avoidDuration);


        if (m_playerState == PlayerState.Dead)
            yield break;


        // 回避終了
        m_movement.Stop();

        m_isInvincible = false;
        m_isAvoiding = false;


        // 移動入力が残っていれば通常移動に戻る
        if (m_velocity.sqrMagnitude > 0.01f)
        {
            ChangeState(PlayerState.Move);
        }
        else
        {
            ChangeState(PlayerState.Idle);
        }
    }


    private IEnumerator AttackCoroutine()
    {
        yield return new WaitForSeconds(m_attackStateDuration);

        if (m_playerState == PlayerState.Attack)
        {
            ChangeState(PlayerState.Idle);
        }
    }

    #endregion


    //==================================================
    // 状態管理・死亡処理
    //==================================================

    #region State

    private void ChangeState(PlayerState newState)
    {
        if (m_playerState == newState) return;

        m_playerState = newState;
    }

    #endregion


    #region Dead

    public void Dead()
    {
        // すでに死亡している場合は何もしない
        if (m_playerState == PlayerState.Dead) return;

        m_playerState = PlayerState.Dead;

        // 入力・ダメージ情報をリセット
        m_velocity = Vector2.zero;
        m_pendingDamage = 0;


        // コルーチン停止
        StopAllCoroutines();
        m_recoverCoroutine = null;


        // 移動停止
        m_movement.Stop();


        // GameManagerへ死亡イベントを通知

        Debug.Log("プレイヤー死亡！", this);
    }

    #endregion

    public void SetHasWeapon(bool hasWeapon)
    {
        m_hasWeapon = hasWeapon;
    }
}
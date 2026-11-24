using System;
using UnityEngine;
using System.Collections.Generic;

public class TestEnemy : Enemy, IDamageable
{
    //案山子なので動かない
    //直線の攻撃をする

    //movementを持つ
    //Attackを持つ
    //Rendererを持つ　flipX
    //

    private Rigidbody2D m_rd;

    private Team m_team;

    public Team Team => m_team;

    private Movement m_movement;
    private EnemyHp m_hpController;

    [SerializeField] private AttackManager m_attackManager;

    private List<AttackBase> m_attacks = new();

    private bool m_isDead = false;

    private void Awake()
    {
        m_rd = GetComponent<Rigidbody2D>();
        m_hpController = GetComponent<EnemyHp>();
        m_movement = new Movement(m_rd, transform, LimitStage);
    }

    public Vector2 LimitStage(Vector2 pos)
    {
        return m_stageManager.LimitMovement(pos);
    }

    public override void InitializeData(EnemyData data)
    {
        base.InitializeData(data);

        m_team = data.Team;
        m_hpController.Initialize(data.HP);
        m_hpController.SetEnemy(this);

        foreach (var attack in data.AttackSettings)
        {
            var attackData = attack.AttackData.CreateAttackClass();

            attackData.Initialize(m_movement, this.transform, m_attackManager);

            m_attacks.Add(attackData);
        }


    }

    private float m_testTime;

    private void Update()
    {
        if (m_isDead) return;


        for (int i = 0; i < m_attacks.Count; i++)
        {
            m_attacks[i].Update();
        }

        m_testTime += Time.deltaTime;

        if(m_testTime > 6f)
        {
            m_testTime = 0;
            SwordAttack();
        }
    }

    //AttackManagerを持ち　実際に攻撃をしてみる
    //HPの処理の確認は必要なし
    [ContextMenu("TestEnemy Sword Attack")]
    public void TestAttack()
    {
        SwordAttack();
    }

    [ContextMenu("TestEnemy Jump Attack")]
    public void TestJumpAttack()
    {
        JumpAttack();
    }

    public void TakeDamage(int damage)
    {
        if(m_isDead) return;

        m_hpController.Damage(damage);
    }

    [EnemyAttackPoint("melee","近接攻撃","近いものに切る")]
    public void SwordAttack()
    {
        m_attacks[0].Execute();
    }

    [EnemyAttackPoint("jump","ジャンプ攻撃","ジャンプして攻撃")]
    public void JumpAttack()
    {
        m_attacks[1].Execute();
    }

    public override void Dead()
    {
        base.Dead();

        m_isDead = true;
    }
}

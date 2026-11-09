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
    [SerializeField] private Team m_team;
    
    public Team Team => m_team;

    private Movement m_movement;

    [SerializeField] private AttackManager m_attackManager;

    private List<AttackBase> m_attacks = new();

    private void Awake()
    {
        m_rd = GetComponent<Rigidbody2D>();
        m_movement = new Movement(m_rd, transform);
    }

    public override void InitializeData(EnemyData data)
    {
        base.InitializeData(data);

        foreach(var attack in data.AttackSettings)
        {
            var attackData = attack.AttackData.CreateAttackClass();

            attackData.Initialize(m_movement, this.transform, m_attackManager);

            m_attacks.Add(attackData);
        }

    }

    private void Update()
    {
        for(int i = 0; i < m_attacks.Count; i++)
        {
            m_attacks[i].Update();
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

    }


    public event Action<Vector2> OnMove;

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
}

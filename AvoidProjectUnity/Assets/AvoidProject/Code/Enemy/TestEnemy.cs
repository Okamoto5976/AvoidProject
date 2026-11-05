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

    private Movement m_movement;

    [SerializeField] private AttackManager m_attackManager;

    private List<IAttack> m_attacks = new();

    private void Awake()
    {
        m_movement = GetComponent<Movement>();
    }

    public override void InitializeData(EnemyData data)
    {
        base.InitializeData(data);

        foreach(var attack in data.AttackSettings)
        {
            m_attacks.Add(attack.AttackData.CreateAttackClass());
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

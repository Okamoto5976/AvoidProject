using System;
using UnityEngine;

public class TestEnemy : Enemy, IDamageable
{
    //案山子なので動かない
    //直線の攻撃をする

    //movementを持つ
    //Attackを持つ
    //Rendererを持つ　flipX
    //

    private Movement m_movement;
    private TestEnemyAttack m_testEnemyAttack;

    [SerializeField] private AttackManager m_attackManager;
    [SerializeField] private Attack m_attack;

    private void Awake()
    {
        m_movement = GetComponent<Movement>();
        m_testEnemyAttack = GetComponent<TestEnemyAttack>();
    }

    //AttackManagerを持ち　実際に攻撃をしてみる
    //HPの処理の確認は必要なし
    [ContextMenu("TestEnemy Attack")]
    public void Attack()
    {
        m_attackManager.Attack(m_attack);
    }


    public void TakeDamage(int damage)
    {

    }


    public event Action<Vector2> OnMove;

    [EnemyAttackPoint("melee","近接攻撃","近いものに切る")]
    public void SowrdAttack()
    {
        
    }

    [EnemyAttackPoint("jump","ジャンプ攻撃","ジャンプして攻撃")]
    public void JumpAttack()
    {

    }
}

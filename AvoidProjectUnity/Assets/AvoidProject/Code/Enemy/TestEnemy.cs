using UnityEngine;

public class TestEnemy : MonoBehaviour, IDamageable
{
    //案山子なので動かない
    //直線の攻撃をする

    //movementを持つ
    //Attackを持つ
    //Rendererを持つ　flipX
    //

    [SerializeField] private AttackManager m_attackManager;
    [SerializeField] private Attack m_attack;

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
}

using UnityEngine;

public class TestEnemyController : MonoBehaviour , IDamageable
{
    [SerializeField] private EnemyHp m_EnemyHp;

    public void TakeDamage(int value)
    {
        m_EnemyHp.Damage(value);
        Debug.Log($"hp = {m_EnemyHp.MaxHp}");
    }
}

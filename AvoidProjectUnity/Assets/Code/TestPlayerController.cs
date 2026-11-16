using UnityEngine;

public class TestPlayerController : MonoBehaviour , IDamageable
{
    [SerializeField] private PlayerHp m_PlayerHp;

    //private void Awake()
    //{
    //    m_PlayerHp.Initialize(5);
    //    m_PlayerHp.InitializeDefenceHp(2);
    //}

    public Team Team { get; }

    public void TakeDamage(int value)
    {
        if (m_PlayerHp.Dhp > 0)
        {
            int damage = m_PlayerHp.DamageDefenceHp(value);
            if (damage > 0) m_PlayerHp.Damage(damage);
        }
        else
        {
            m_PlayerHp.Damage(value);
        }
        Debug.Log($"hp = {m_PlayerHp.Hp}");
        Debug.Log($"dhp = {m_PlayerHp.Dhp}");
    }
    public void TakeHeal(int value)
    {
        m_PlayerHp.Heal(value);
        Debug.Log($"hp = {m_PlayerHp.Hp}");
        Debug.Log($"dhp = {m_PlayerHp.Dhp}");
    }
}

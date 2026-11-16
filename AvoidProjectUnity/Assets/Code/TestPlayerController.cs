using UnityEngine;

public class TestPlayerController : MonoBehaviour , IDamageable
{
    [SerializeField] private PlayerHp m_PlayerHp;


    public Team Team { get; }

    public void TakeDamage(int value)
    {
        m_PlayerHp.Damage(value);
        //Debug.Log($"hp = {m_PlayerHp.MaxHp}");
    }
}

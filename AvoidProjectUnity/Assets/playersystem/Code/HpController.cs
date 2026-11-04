using UnityEngine;
public abstract class HpController : MonoBehaviour
{
    private int m_MaxHp = 100;
    private int m_Hp;
    private bool m_Alive = true;

    public int Hp {  get { return m_Hp; } }

    public void Initialize(int hpValue)
    {
        m_MaxHp = hpValue;
        m_Hp = m_MaxHp;
    }

    public void Damage(int damage)
    {
        m_Hp -= damage;
        if (m_Hp <= 0)
        {
            Dead();
        }
    }

    public void Heal(int value)
    {
        if (m_Alive == false) return;

        m_Hp += value;
        if (m_Hp >= m_MaxHp)
        {
            m_Hp = m_MaxHp;
        }
    }

    protected virtual void Dead()
    {
        Debug.Log("Entity is dead");
        m_Alive = false;
    }
}
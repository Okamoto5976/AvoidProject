using UnityEngine;
public class HpController : MonoBehaviour
{
    private int m_MaxHp;
    private int m_Hp;
    private int m_DefenceHp;
    private bool m_Alive = true;

    public int Hp => m_Hp;
    public int Dhp => m_DefenceHp;

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

    /// <summary>
    /// 消費HPにダメージを与える関数
    /// </summary>
    /// <param name="damage">ダメージ</param>
    /// <returns>消費HP分を超えたダメージ、超えていない場合は0を返す</returns>
    public int DamageDefenceHp(int damage)
    {
        m_DefenceHp -= damage;
        if (m_DefenceHp < 0)
        {
            int rest = -m_DefenceHp;
            m_DefenceHp = 0;
            return rest;
        }
        return 0;
    }

    public void InitializeDefenceHp(int dhp)
    {
        m_DefenceHp = dhp;
    }

    protected virtual void Dead()
    {
        if(m_Alive == false) return;

        Debug.Log("Entity is dead");
        m_Alive = false;
    }
}
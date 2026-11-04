using UnityEngine;
public class HpController : MonoBehaviour
{

    private int m_maxHp;

    public int MaxHp {  get { return m_maxHp; } }

    public void Initialize(int hpValue)
    {
        m_maxHp = hpValue;
    }

    public void Damage(int damage)
    {
        m_maxHp -= damage;
        if (m_maxHp <= 0)
        {
            Dead();
        }
    }

    protected virtual void Dead()
    {
        Debug.Log("Entity is dead");
    }
}

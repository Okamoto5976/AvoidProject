using UnityEngine;
public abstract class HpController : MonoBehaviour
{
    private int m_maxHp = 100;

    public int MaxHp {  get { return m_maxHp; } }

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

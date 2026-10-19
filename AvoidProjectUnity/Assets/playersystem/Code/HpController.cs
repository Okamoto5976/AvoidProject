using UnityEngine;

public class HpController : MonoBehaviour
{
    private int m_maxHp = 100;
  
    
     
    public void Damage(int damage)
    {
        m_maxHp -= damage;
        if (m_maxHp <= 0)
        {
            Dead();
        }
    }

    private void Dead()
    {
        Debug.Log("Player is dead");
    }


}

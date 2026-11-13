using UnityEngine;

public class ColliderAttack : MonoBehaviour
{
    private int m_damage = 0;
    private Team m_team;

    public void Initialize(int damage, Team team)
    {
        m_damage = damage;
        m_team = team;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        IDamageable entity = other.gameObject.GetComponentInParent<IDamageable>();

        if(entity == null)
        {
            //Debug.Log("Not idamageble");
            return;
        }

        if (entity.Team == m_team)
        {
            //Debug.Log($"my team is {m_team} : entity team is {entity.Team}");
            return;
        }
 

        entity.TakeDamage(m_damage);
    }
}
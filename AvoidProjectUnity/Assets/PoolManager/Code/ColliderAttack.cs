using UnityEngine;

public class ColliderAttack : MonoBehaviour
{
    public int m_damage = 0;
    public Team m_team;

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        IDamageable entity = other.gameObject.GetComponentInParent<IDamageable>();

        if (entity.Team == m_team) return;
 

        entity.TakeDamage(m_damage);
    }
}
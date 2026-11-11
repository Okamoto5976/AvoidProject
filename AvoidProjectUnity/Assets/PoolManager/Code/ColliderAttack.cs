using UnityEngine;

public class ColliderAttack : MonoBehaviour
{
    public int m_damage = 0;
    public Team m_team;

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log(other.gameObject.name);
        IDamageable entity = other.gameObject.GetComponentInParent<IDamageable>();
        //Player�ɖ��G������𔻒肪��������return����@�ォ��ǉ�
        if (entity.Team == m_team) return;
 

        entity.TakeDamage(m_damage);
    }
}
using UnityEngine;

[CreateAssetMenu(fileName = "SwordAttackData", menuName = "Scriptable Objects/Data/MeleeAttackData")]
public class MeleeAttackData : AttackData
{
    [SerializeField] private float m_radius;
    [SerializeField] private float m_size;


}

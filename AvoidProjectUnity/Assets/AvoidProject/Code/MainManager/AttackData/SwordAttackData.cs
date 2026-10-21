using UnityEngine;

[CreateAssetMenu(fileName = "SwordAttackData", menuName = "Scriptable Objects/Data/SwordAttackData")]
public class SwordAttackData : AttackData
{
    [SerializeField] private float m_radius;
    [SerializeField] private float m_size;

    public override IAttack CreateAttack()
    {
        return new SwordAttack(this);
    }
}

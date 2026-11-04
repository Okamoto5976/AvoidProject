using UnityEngine;

public class MeleeAttack : IAttack
{
    private readonly MeleeAttackData m_data;

    public MeleeAttack(MeleeAttackData data)
    {
        m_data = data;
    }

    public void Execute(AttackContext context)
    {

    }
}

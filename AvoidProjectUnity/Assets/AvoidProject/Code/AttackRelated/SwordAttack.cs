using UnityEngine;

public class SwordAttack : IAttack
{
    private readonly SwordAttackData m_data;

    public SwordAttack(SwordAttackData data)
    {
        m_data = data;
    }

    public void Execute(AttackContext context)
    {

    }
}

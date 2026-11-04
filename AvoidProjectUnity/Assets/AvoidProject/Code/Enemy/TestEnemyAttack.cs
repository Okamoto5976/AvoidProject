using UnityEngine;
using System;

public class TestEnemyAttack : MonoBehaviour
{
    private MeleeAttack m_attack;
    [SerializeField] private MeleeAttackData m_attackData;

    public event Action<Vector2> OnMove;

    private void Awake()
    {
        m_attack = new(m_attackData);
    }

    public void SowrdAttack()
    {
        m_attack.Execute();
    }
}

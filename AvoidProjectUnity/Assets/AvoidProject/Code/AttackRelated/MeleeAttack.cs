using UnityEngine;
using System.Collections.Generic;

public class MeleeAttack : AttackBase
{
    private readonly MeleeAttackData m_data;

    private List<Action> m_actions = new();

    private int m_currentIndex = 0;

    private Attack m_attack;

    public MeleeAttack(MeleeAttackData data)
    {
        m_data = data;

        m_attack = data.m_attackContext;
        foreach (var actionData in data.ActionList)
        {
            m_actions.Add(actionData.CreateAction(this));
        }
    }

    public override void Execute()
    {
        m_currentIndex = 0;
        m_actions[m_currentIndex].Execute();
    }

    public override void Update()
    {
        if (m_currentIndex >= m_actions.Count) return;

        m_actions[m_currentIndex].Update();

        if (!m_actions[m_currentIndex].IsComplete()) return;

        m_currentIndex++;

        if(m_currentIndex < m_actions.Count)
        {
            m_actions[m_currentIndex].Execute();
        }
    }

    //MovementClass
    //AttackManagerClass‚ª‚¢‚é
    //Transform‚ª‚¢‚é

    public override void Attack()
    {
        //Debug.Log("AttackCollider");
        m_attack.Position = m_transform.position;
        m_attackManager.Attack(m_attack);
    }
}

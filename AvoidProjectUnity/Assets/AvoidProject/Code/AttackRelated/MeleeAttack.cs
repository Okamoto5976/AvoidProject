using UnityEngine;
using System.Collections.Generic;

public class MeleeAttack : IAttack
{
    private readonly MeleeAttackData m_data;

    private List<Action> m_actions = new();

    private int m_currentIndex = 0;

    public MeleeAttack(MeleeAttackData data)
    {
        m_data = data;
        foreach (var actionData in data.ActionList)
        {
            m_actions.Add(actionData.CreateAction());
        }
    }

    public void Execute()
    {
        m_currentIndex = 0;
        m_actions[m_currentIndex].Execute();
    }

    public void Update()
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
}

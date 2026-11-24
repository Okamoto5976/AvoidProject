using UnityEngine;
using System;

public abstract class Enemy : MonoBehaviour
{
    protected StageManager m_stageManager;

    public event Action m_onDeadEvent;

    public void SetStageManager(StageManager component)
    {
        m_stageManager = component;

    }

    public virtual void Dead()
    {
        m_onDeadEvent?.Invoke();
    }

    public virtual void InitializeData(EnemyData data)
    {
        if (!data.MatchesEnemyType(GetType()))
        {
            Debug.LogError(
                $"Enemy‚ÌŒ^‚ªˆê’v‚µ‚Ü‚¹‚ñBEnemy: {GetType().FullName}",
                this
            );
            return;
        }

        Debug.Log("EnemyData‚Ì‰Šú‰»¬Œ÷");
    }
}

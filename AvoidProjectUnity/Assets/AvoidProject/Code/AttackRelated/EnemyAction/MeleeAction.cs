using UnityEngine;

public class MeleeAction : Action
{
    private readonly MeleeActionData m_actionData;

    public MeleeAction(MeleeActionData data, AttackBase attack)
    {
        m_actionData = data;

        m_attackClass = attack;
    }

    public override void Execute()
    {
        //Debug.Log("çUåÇ");

        m_attackClass.Attack();

        m_isComplete = true;

    }

    public override void Update()
    {
        
    }

    public override bool IsComplete()
    {
        return m_isComplete;
    }
}

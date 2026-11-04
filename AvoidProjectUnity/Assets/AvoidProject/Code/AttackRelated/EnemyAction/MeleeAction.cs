using UnityEngine;

public class MeleeAction : Action
{
    private readonly MeleeActionData m_actionData;

    public MeleeAction(MeleeActionData data)
    {
        m_actionData = data;
    }

    public override void Execute()
    {
        
    }

    public override void Update()
    {
        
    }

    public override bool IsComplete()
    {
        return m_isComplete;
    }
}

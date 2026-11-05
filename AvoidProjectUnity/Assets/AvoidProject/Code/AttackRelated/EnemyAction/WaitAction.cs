using UnityEngine;

public class WaitAction : Action
{
    private readonly WaitActionData m_actionData;

    public WaitAction(WaitActionData data)
    {

    }

    public override void Execute()
    {
        Debug.Log("‘Ò‚Â");
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

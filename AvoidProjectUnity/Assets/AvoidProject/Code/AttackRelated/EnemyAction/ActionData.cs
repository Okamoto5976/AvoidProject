using UnityEngine;

[System.Serializable]
public abstract class ActionData
{
    public abstract Action CreateAction();
}

[System.Serializable]
public class MeleeActionData : ActionData
{
    public float a;//発動タイミング

    public override Action CreateAction()
    {
        return new MeleeAction(this);
    }
}

[System.Serializable]
public class WaitActionData : ActionData
{
    public override Action CreateAction()
    {
        return new WaitAction(this);
    }
}
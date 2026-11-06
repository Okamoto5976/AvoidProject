using UnityEngine;

[System.Serializable]
public abstract class ActionData
{
    public abstract Action CreateAction(IAttack attack);
}

[System.Serializable]
[ActionDataFor(typeof (MeleeAttackData))]
public class MeleeActionData : ActionData
{
    public float a;//発動タイミング

    public override Action CreateAction(IAttack attack)
    {
        return new MeleeAction(this, attack);
    }
}

[System.Serializable]
[ActionDataFor(typeof (MeleeAttackData))]
[ActionDataFor(typeof (JumpAttackData))]
public class WaitActionData : ActionData
{
    public override Action CreateAction(IAttack attack)
    {
        return new WaitAction(this);
    }
}
using UnityEngine;

[System.Serializable]
public abstract class ActionData
{
    public abstract EnemyAction CreateAction(AttackBase attack);
}

[System.Serializable]
[ActionDataFor(typeof (MeleeAttackData))]
public class MeleeActionData : ActionData
{
    public float a;//発動タイミング

    public override EnemyAction CreateAction(AttackBase attack)
    {
        return new MeleeAction(this, attack);
    }
}

[System.Serializable]
[ActionDataFor(typeof (MeleeAttackData))]
[ActionDataFor(typeof (JumpAttackData))]
public class WaitActionData : ActionData
{
    public override EnemyAction CreateAction(AttackBase attack)
    {
        return new WaitAction(this);
    }
}
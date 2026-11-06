using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class AttackData
{
    public abstract IAttack CreateAttackClass();
}

[System.Serializable]
public class MeleeAttackData : AttackData
{
    public Attack m_attackContext;

    public override IAttack CreateAttackClass()
    {
        return new MeleeAttack(this);
    }

    //ActionListを持つ
    //どのActionを持てるかがあるので　並び替えを行えるようにする　Editorでそれぞれ表示可能のものを決めておく

    [SerializeReference]
    private List<ActionData> m_actionList = new();

    public List<ActionData> ActionList => m_actionList;

    public void PlayerTestSetAction()
    {
        m_actionList.Clear();
        m_actionList.Add(new MeleeActionData());
    }
}

[System.Serializable]
public class JumpAttackData : AttackData
{
    public override IAttack CreateAttackClass()
    {
        return null;
    }
}
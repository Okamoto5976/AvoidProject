using UnityEngine;
using System.Collections.Generic;

[System.Serializable]
public abstract class AttackData
{
}

[System.Serializable]
public class MeleeAttackData : AttackData
{
    [SerializeField] private AttackContext m_attackContext;

    //ActionListを持つ
    //どのActionを持てるかがあるので　並び替えを行えるようにする　Editorでそれぞれ表示可能のものを決めておく

    [SerializeReference]
    private List<ActionData> m_actionList = new();
}

[System.Serializable]
public class JumpAttackData : AttackData
{
}
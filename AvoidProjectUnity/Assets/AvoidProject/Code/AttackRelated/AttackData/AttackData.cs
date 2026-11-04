using System;
using UnityEngine;

[Serializable]
public abstract class AttackData
{
    public string name;
}

[Serializable]
public class MeleeAttackData : AttackData
{
    public int attack;

    //ActionListを持つ
    //どのActionを持てるかがあるので　並び替えを行えるようにする　Editorでそれぞれ表示可能のものを決めておく
}

[Serializable]
public class JumpAttackData : AttackData
{
    public int jump;
}
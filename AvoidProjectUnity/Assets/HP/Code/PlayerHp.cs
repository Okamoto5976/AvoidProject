using JetBrains.Annotations;
using UnityEngine;

public class PlayerHp : HpController
{
    protected override void Dead()
    {
        base.Dead();

        Debug.Log("Enemy win");
    }
}

using UnityEngine;

public class EnemyHp : HpController
{
    protected override void Dead()
    {
        base.Dead();

        Debug.Log("Player win");
    }
}

using UnityEngine;

public class EnemyHp : HpController
{
    private Enemy m_enemy;

    public void SetEnemy(Enemy enemy)
    {
        m_enemy = enemy;
    }

    protected override void Dead()
    {
        base.Dead();

        m_enemy.Dead();
    }
}

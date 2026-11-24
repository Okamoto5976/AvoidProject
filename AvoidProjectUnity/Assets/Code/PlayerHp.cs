using UnityEngine;

public class PlayerHp : HpController
{
    private PlayerController m_player;

    public void SetPlayer(PlayerController player)
    {
        m_player = player;
    }

    protected override void Dead()
    {
        base.Dead();

        m_player.Dead();
    }
}

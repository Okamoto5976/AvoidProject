using UnityEngine;

public class TEnemy : MonoBehaviour
{
    [EnemyAttackPoint("normalAttack","’ÊíUŒ‚","•à‚«‚È‚ª‚çUŒ‚‚µ‚Ä‚­‚é")]
    public void NormalAttack()
    {
        Debug.Log("NormalAttack");
    }

    [EnemyAttackPoint("jumpAttack","ƒWƒƒƒ“ƒvUŒ‚","ƒWƒƒƒ“ƒv‚µ‚ÄUŒ‚")]
    public void JumpAttack()
    {
        Debug.Log("JumpAttack");
    }
}

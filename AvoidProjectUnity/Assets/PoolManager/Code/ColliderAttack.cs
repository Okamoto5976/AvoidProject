using UnityEngine;

public class ColliderAttack : MonoBehaviour
{
    public int Damage = 0;

    void OnTriggerStay2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        TestPlayerController playerController = other.gameObject.GetComponent<TestPlayerController>();
        //Player‚É–³“G”»’è‚â‰ñ”ğ”»’è‚ª‚ ‚Á‚½‚çreturn‚·‚é@Œã‚©‚ç’Ç‰Á
        playerController.TakeDamage(Damage);
    }
}
using UnityEngine;

public class ColliderAttack : MonoBehaviour
{
    public int Damage = 0;

    void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log(other.gameObject.name);
        TestPlayerController playerController = other.gameObject.GetComponent<TestPlayerController>();
        playerController.TakeDamage(Damage);
    }
}
using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private AttackManager m_AttackManager;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    [SerializeField] private bool random;
    [SerializeField] private Attack sampleAttack;

    private void Update()
    {
        //test
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (random) sampleAttack = RandomPosition();

            m_AttackManager.Attack(sampleAttack);
        }
    }

    private Attack RandomPosition()
    {
        Attack attack = sampleAttack.Clone();
        attack.Position = new Vector2(Random.Range(-5f, 5f), Random.Range(-2f, 2f));
        return attack;
    }

    void OnTriggerEnter2D(Collider2D other)
    {
        //Debug.Log($"TriggeräJén: {other.name}");
        m_SpriteRenderer.color = Color.black;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log($"TriggerèIóπ: {other.name}");
        m_SpriteRenderer.color = Color.white;
    }
}
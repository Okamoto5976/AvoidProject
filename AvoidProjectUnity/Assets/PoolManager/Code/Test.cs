using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private AttackManager m_AttackManager;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    [SerializeField] private bool random;
    [SerializeField] private Attack test;
    private void Update()
    {
        //test
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (random) m_AttackManager.Attack(RandomAttack());
            else m_AttackManager.Attack(test);
        }
    }

    private Attack RandomAttack()
    {
        Attack attack = new Attack();
        attack.ShapeType = (ShapeType)Random.Range(0, 2);
        float value = Random.Range(1f, 3f);
        attack.Size = new Vector2(value, value);
        attack.Position = new Vector2(Random.Range(-5f, 5f), Random.Range(-2f, 2f));
        attack.StartupTime = Random.Range(1f, 5f);
        //attack.StartupTime = 0;
        attack.ActiveTime = Random.Range(1f, 5f);
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

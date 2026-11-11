using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{
    [SerializeField] private AttackManager m_AttackManager;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    [SerializeField] private bool random;
    [SerializeField] private Attack testAttack;

    [SerializeField] private ColliderManager m_ColliderManager;
    [SerializeField] private AttackViewManager m_AttackViewManager;
    [SerializeField] private PoolManager m_AttackViewPool;
    [SerializeField] private ColliderMediationPool m_ColliderMediationPool;

    private void Awake()
    {
        ComponentInitialize();
    }

    private void ComponentInitialize()
    {
        //AttackManagerSet
        m_AttackManager.SetColliderManager(m_ColliderManager);
        m_AttackManager.SetAttackViewManager(m_AttackViewManager);

        //AttackViewManagerSet
        m_AttackViewManager.SetAttackViewPool(m_AttackViewPool);
        m_AttackViewManager.SetAttackManager(m_AttackManager);

        //ColliderManagerSet
        m_ColliderManager.SetColliderMediationPool(m_ColliderMediationPool);
    }

    private void Update()
    {
        //test
        if (Mouse.current.leftButton.wasPressedThisFrame)
        {
            if (random) RandomAttack();

            m_AttackManager.Attack(testAttack);
        }
    }

    private void RandomAttack()
    {
        testAttack.ShapeType = (ShapeType)Random.Range(0, 2);
        float value = Random.Range(1f, 3f);
        testAttack.Size = new Vector2(value, value);
        testAttack.Position = new Vector2(Random.Range(-5f, 5f), Random.Range(-2f, 2f));
        //testAttack.StartupTime = Random.Range(1f, 5f);
        //testAttack.StartupTime = 0;
        //testAttack.ActiveTime = Random.Range(1f, 5f);
        //testAttack.Damage = Random.Range(1, 10);
    }

    void OnTriggerStay2D(Collider2D other)
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

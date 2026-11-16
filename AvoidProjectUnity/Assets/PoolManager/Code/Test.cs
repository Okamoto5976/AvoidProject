using UnityEngine;
using UnityEngine.InputSystem;

public class Test : MonoBehaviour
{

    [Header("攻撃テスト用")]
    [SerializeField] private AttackManager m_AttackManager;
    [SerializeField] private SpriteRenderer m_SpriteRenderer;

    [SerializeField] private bool random;
    [SerializeField] private Attack sampleAttack;

    [Header("ComponentInitalize用")]
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
            if (random) sampleAttack = RandomPosition();

            Debug.Log(sampleAttack.Position);
            transform.position = sampleAttack.Position;
            m_AttackManager.Attack(sampleAttack);
        }
    }

    private Attack RandomPosition()
    {
        Attack attack = sampleAttack.Clone();
        attack.Position = new Vector2(Random.Range(-5f, 5f), Random.Range(-2f, 2f));
        return attack;
    }

    void OnTriggerStay2D(Collider2D other)
    {
        //Debug.Log($"TriggerŠJŽn: {other.name}");
        m_SpriteRenderer.color = Color.black;
    }
    void OnTriggerExit2D(Collider2D other)
    {
        //Debug.Log($"TriggerI—¹: {other.name}");
        m_SpriteRenderer.color = Color.white;
    }
}
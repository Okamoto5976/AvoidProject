using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class EnemyHPTest : MonoBehaviour
{
    [SerializeField] private UIManager m_uiManager;
    [SerializeField] private int m_maxHP = 100;
    [SerializeField] private int m_damage = 10;

    private int m_currentHP;

    private void Start()
    {
        m_currentHP = m_maxHP;
        m_uiManager.SetupEnemyHP(m_maxHP);
        m_uiManager.UpdateEnemyHP(m_currentHP);
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return;
        if (Keyboard.current.spaceKey.wasPressedThisFrame) TakeDamage(m_damage);
#else
        if (Input.GetKeyDown(KeyCode.Space)) TakeDamage(m_damage);
#endif
    }

    public void TakeDamage(int amount)
    {
        if (m_currentHP <= 0) return;
        SetHP(m_currentHP - amount);
    }

    private void SetHP(int value)
    {
        m_currentHP = Mathf.Clamp(value, 0, m_maxHP);
        m_uiManager.UpdateEnemyHP(m_currentHP);

        if (m_currentHP <= 0)
        {
            m_uiManager.ShowPanel(UIPanelType.GameClear);
        }
    }
}
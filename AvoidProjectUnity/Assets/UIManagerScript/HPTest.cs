using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif

public class HPTest : MonoBehaviour
{
    [SerializeField] private UIManager m_uiManager;
    [SerializeField] private int m_maxHP = 2; // ÉnÅ[Ég1å¬ = 2

    private int m_currentHP;

    private void Start()
    {
        m_currentHP = m_maxHP;
        m_uiManager.SetupHearts(m_maxHP);
        m_uiManager.UpdateHearts(m_currentHP);
    }

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        if (Keyboard.current == null) return;
        if (Keyboard.current.dKey.wasPressedThisFrame) TakeDamage(3);
        if (Keyboard.current.hKey.wasPressedThisFrame) Heal(1);
#else
        if (Input.GetKeyDown(KeyCode.D)) TakeDamage(1);
        if (Input.GetKeyDown(KeyCode.H)) Heal(1);
#endif
    }

    public void TakeDamage(int amount)
    {
        if (m_currentHP <= 0) return;
        SetHP(m_currentHP - amount);
    }

    public void Heal(int amount)
    {
        if (m_currentHP <= 0) return;
        SetHP(m_currentHP + amount);
    }

    private void SetHP(int value)
    {
        m_currentHP = Mathf.Clamp(value, 0, m_maxHP);
        m_uiManager.UpdateHearts(m_currentHP);

        if (m_currentHP <= 0)
        {
            m_uiManager.ShowPanel(UIPanelType.GameOver);
        }
    }
}
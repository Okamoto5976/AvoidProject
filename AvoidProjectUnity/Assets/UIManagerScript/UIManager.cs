using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public enum UIPanelType
{
    None,
    GameClear,
    GameOver
}

public class UIManager : MonoBehaviour
{
    //Gameover,GameStartのPanelを管理(呼び出せるぜ)
    [SerializeField] private GameObject m_gameStartPanel;
    [SerializeField] private GameObject m_gameOverPanel;

    //HP表示(ハートの見た目はHeartViewのPrefab側で自由に変えられる)
    [SerializeField] private HeartView m_heartPrefab;
    [SerializeField] private Transform m_heartParent;
    [SerializeField] private int m_hpPerHeart = 2;

    //敵HPの表示
    [SerializeField] private Slider m_enemyHPSlider;

    private readonly List<HeartView> m_hearts = new List<HeartView>();

    private Dictionary<UIPanelType, GameObject> panels;

    private int HpPerHeart => Mathf.Max(1, m_hpPerHeart);

    private void Awake()
    {
        panels = new Dictionary<UIPanelType, GameObject>
        {
            { UIPanelType.GameClear, m_gameStartPanel },
            { UIPanelType.GameOver , m_gameOverPanel  }
        };

        ShowPanel(UIPanelType.None);
    }

    public void ShowPanel(UIPanelType type)
    {
        foreach (var pair in panels)
        {
            if (pair.Value == null) continue;
            pair.Value.SetActive(pair.Key == type);
        }
    }

    public void HidePanel(UIPanelType type)
    {
        if (panels.TryGetValue(type, out var panel) && panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void HideAll() => ShowPanel(UIPanelType.None);

    public void ShowGameStart() => ShowPanel(UIPanelType.GameClear);
    public void ShowGameOver() => ShowPanel(UIPanelType.GameOver);

    public void SetupHearts(int maxHP)
    {
        foreach (var heart in m_hearts)
        {
            Destroy(heart.gameObject);
        }
        m_hearts.Clear();

        int heartCount = Mathf.CeilToInt(maxHP / (float)HpPerHeart);
        for (int i = 0; i < heartCount; i++)
        {
            m_hearts.Add(Instantiate(m_heartPrefab, m_heartParent));
        }
    }
    public void UpdateHearts(int currentHP)
    {
        for (int i = 0; i < m_hearts.Count; i++)
        {
            float ratio = (currentHP - i * HpPerHeart) / (float)HpPerHeart;
            m_hearts[i].SetFill(ratio);
        }
    }

    // 敵のHP処理
    public void SetupEnemyHP(int maxHP)
    {
        m_enemyHPSlider.minValue = 0;
        m_enemyHPSlider.maxValue = maxHP;
        m_enemyHPSlider.value = maxHP;
    }

    public void UpdateEnemyHP(int currentHP)
    {
        m_enemyHPSlider.value = currentHP;
    }
}
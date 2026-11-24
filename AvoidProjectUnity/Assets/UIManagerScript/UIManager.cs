using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public enum UIPanelType
{
    None,
    GameResult,
    GameOver
}

public class UIManager : MonoBehaviour
{
    //Gameover,GameStartのPanelを管理(呼び出せるぜ)
    [SerializeField] private GameObject m_gameResultPanel;
    [SerializeField] private GameObject m_gameOverPanel;

    //HP表示(ハートの見た目はHeartViewのPrefab側で自由に変えられる)
    [SerializeField] private PlayerHPUI m_playerHeartPrefab;
    [SerializeField] private Transform m_heartListContent;

    //敵HPの表示
    [SerializeField] private Image m_enemyHPFilled;
    private int m_maxHP;

    [SerializeField] private TMP_Text m_potionCountText;

    private readonly List<PlayerHPUI> m_hearts = new List<PlayerHPUI>();

    private Dictionary<UIPanelType, GameObject> m_panels;

    private void Awake()
    {
        m_panels = new Dictionary<UIPanelType, GameObject>
        {
            { UIPanelType.GameResult, m_gameResultPanel },
            { UIPanelType.GameOver , m_gameOverPanel  }
        };

        ShowPanel(UIPanelType.None);
    }

    public void ShowPanel(UIPanelType type)
    {
        foreach (var pair in m_panels)
        {
            if (pair.Value == null) continue;
            pair.Value.SetActive(pair.Key == type);
        }
    }

    public void HidePanel(UIPanelType type)
    {
        if (m_panels.TryGetValue(type, out var panel) && panel != null)
        {
            panel.SetActive(false);
        }
    }

    public void HideAll() => ShowPanel(UIPanelType.None);

    public void ShowGameClear() => ShowPanel(UIPanelType.GameResult);
    public void ShowGameOver() => ShowPanel(UIPanelType.GameOver);

    public void SetUpHearts(int maxHP, int dhp)
    {
        if(maxHP < 0)
        {
            Debug.LogError("PlayerHPがありません", this);
        }
        else if(maxHP < dhp)
        {
            Debug.LogError("DHPがHPより大きいため続けれません");
        }

        //Debugのため　SetUpが呼ばれたら
        if (m_hearts.Count > 0)
        {
            //もしhpより少なかったら
            //引いた分追加
            if (m_hearts.Count < maxHP)
            {
                int num = maxHP - m_hearts.Count;

                for (int i = 0; i < num; i++)
                {
                    var obj = Instantiate(m_playerHeartPrefab, m_heartListContent);

                    var component = obj.GetComponent<PlayerHPUI>();

                    m_hearts.Add(component);
                }
            }
            //もしhpより多かったら
            //引いた分Listの上から削除
            else if (m_hearts.Count > maxHP)
            {
                int num = m_hearts.Count - maxHP;

                for (int i = num; i < 1; i--)
                {
                    var obj = m_hearts[i].gameObject;

                    m_hearts.RemoveAt(i);

                    Destroy(obj);
                }
            }
            else if (m_hearts.Count == maxHP)
            {

            }
        }
        else
        {
            for (int i = 0; i < maxHP; i++)
            {
                var obj = Instantiate(m_playerHeartPrefab, m_heartListContent);

                var component = obj.GetComponent<PlayerHPUI>();

                m_hearts.Add(component);
            }
        }

        UpdateHearts(maxHP, dhp);
    }
    public void UpdateHearts(int hp, int dhp)//ハートが３ hpが2 dhpが1
    {
        for(int i = 1; i <= m_hearts.Count; i++)
        {
            if(i > dhp)
            {
                m_hearts[i - 1].ViewGreenHeart(false);
            }
            else
            {
                m_hearts[i - 1].ViewGreenHeart(true);
                continue;
            }

            if (i > hp)
            {
                m_hearts[i - 1].ViewRedHeart(false);

                continue;
            }



            m_hearts[i - 1].ViewRedHeart(true);
        }
    }

    // 敵のHP処理
    public void SetUpEnemyHP(int maxHP)
    {
        //m_enemyHPSlider.minValue = 0;
        //m_enemyHPSlider.maxValue = maxHP;
        m_enemyHPFilled.fillAmount = 1f;
        m_maxHP = maxHP;
    }

    //dhpは使ってない
    public void UpdateEnemyHP(int hp, int dhp)
    {
        if(dhp > 0)
        {
            Debug.LogWarning("Enemy has DefenceHP, that's not necessary");
        }

        float rate = (float)hp / m_maxHP;
        Debug.Log($"Enemy UI Rate : {rate}  HP : {hp} / {m_maxHP}");
        m_enemyHPFilled.fillAmount = rate;
    }

    public void SetPotionCount(int num)
    {
        m_potionCountText.text = num.ToString();
    }

}
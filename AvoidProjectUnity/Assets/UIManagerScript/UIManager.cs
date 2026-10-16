using System.Collections.Generic;
using UnityEngine;

public enum UIPanelType
{
    None,
    GameClear,
    GameOver
}

public class UIManager : MonoBehaviour
{
    [Header("Panels")]
    [SerializeField] private GameObject m_gameStartPanel;
    [SerializeField] private GameObject m_gameOverPanel;

    private Dictionary<UIPanelType, GameObject> panels;

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
}
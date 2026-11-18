using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuffManager : MonoBehaviour
{
    [SerializeField] private DebuffData m_DebuffData;
    [SerializeField] private List<TextMeshProUGUI> m_ButtonTexts;
    [SerializeField] private TextMeshProUGUI m_DebuffNameText;
    [SerializeField] private TextMeshProUGUI m_DebuffRewardText;
    [SerializeField] private TextMeshProUGUI m_DebuffActiveText;
    //[SerializeField] private List<DebuffElement> m_Debuffs;
    //[SerializeField] private List<Debuff> m_AllDebuffs;
    private List<int> m_DebuffList = new();     //後にランタイムデータに移動
    private int m_SelectIndex = 0;
    private List<Debuff> m_DebuffsInCategory = new();
    private int m_CategoryIndex = 0;

    private void Awake()    {
        Initialize();
    }

    private void Initialize()
    {
        m_DebuffData.Initialize();
        CategorySet();
    }

    public void OnChangeCategory(int direction)
    {
        m_CategoryIndex = GetCategoryIndex(m_CategoryIndex, direction);
        CategorySet();
    }

    public void OnShowDebuff(int index)
    {
        m_SelectIndex = index;
        Debuff debuff = m_DebuffsInCategory[m_SelectIndex];
        m_DebuffNameText.text = debuff.Name;
        m_DebuffRewardText.text = $"${debuff.Reward}";
        if (ActiveCheck(debuff.Id)) 
        {
            m_DebuffActiveText.text = "CompletedSet";
        }
        else 
        {
            m_DebuffActiveText.text = "UncompletedSet";
        }
    }

    public void OnEnterDebuff()
    {
        int id = m_DebuffsInCategory[m_SelectIndex].Id;
        bool active = ActiveCheck(id);
        if (active)
        {
            m_DebuffList.Remove(id);
        }
        else
        {
            m_DebuffList.Add(id);
        }
        OnShowDebuff(m_SelectIndex);
    }

    public void CategorySet()
    {
        DebuffCategory category = (DebuffCategory)m_CategoryIndex;
        m_DebuffsInCategory = m_DebuffData.GetDebuffsInCategory(category);
        for (int i = 0; i < m_ButtonTexts.Count; i++)
        {
            m_ButtonTexts[i].text = m_DebuffsInCategory[i].Name;
        }
    }

    private int GetCategoryIndex(int now, int direction)
    {
        int index = -1;
        if (direction > 0)
        {
            index = now + 1;
            if (index >= Enum.GetValues(typeof(DebuffCategory)).Length)
            {
                index = 0;
            }
        }
        else if (direction < 0) 
        {
            index = now - 1;
            if (index <= -1)
            {
                index = Enum.GetValues(typeof(DebuffCategory)).Length - 1;
            }
        }
        return index;
    }
    
    public bool ActiveCheck(int checkId)
    {
        foreach (int id in m_DebuffList)
        {
            if (id == checkId)
            {
                return true;
            }
        }
        return false;
    }
}
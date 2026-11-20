using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DebuffPresetManager : MonoBehaviour
{
    [SerializeField] private DebuffManager m_DebuffManager;
    [SerializeField] private DebuffData m_DebuffData;

    [SerializeField] private GameObject m_PresetPanel;
    [SerializeField] private TextMeshProUGUI m_PresetContent;
    private int m_PresetIndex;

    public void OnEditPreset(int index)
    {
        m_PresetIndex = index;
        m_PresetContent.text = null;
        List<int> preview = m_DebuffData.GetPreset(m_PresetIndex);

        if (preview != null)
        {
            if (preview.Count == 0)
            {
                m_PresetContent.text = "Preset no set";
            }
            foreach (int id in preview)
            {
                Debuff debuff = m_DebuffData.GetDebuff(id);
                if (debuff == null) continue;
                m_PresetContent.text += $"{debuff.Name}\n";
            }
        }
        m_PresetPanel.SetActive(true);
    }

    public void OnSetPreset()
    {
        List<int> debuffs = new List<int>(m_DebuffManager.GetDebuffList());
        m_DebuffData.SetPreset(m_PresetIndex, debuffs);
        m_PresetPanel.SetActive(false);
        Debug.Log("プリセットを設定しました");
    }

    public void OnGetPreset()
    {
        List<int> debuffs = m_DebuffData.GetPreset(m_PresetIndex);
        m_DebuffManager.SetDebuffList(debuffs);
        m_DebuffManager.UpdateShowDebuff();
        m_PresetPanel.SetActive(false);
        Debug.Log("プリセットを反映しました");
    }

    public void OnDeletePreset()
    {
        if (!m_DebuffData.DeletePreset(m_PresetIndex))
        {
            Debug.Log("プリセットを削除出来ませんでした");
            return;
        }
        m_PresetPanel.SetActive(false);
        Debug.Log("プリセットを削除しました");
    }
}
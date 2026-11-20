using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebuffData", menuName = "Scriptable Objects/DebuffData")]
public class DebuffData : ScriptableObject
{
    [SerializeField] private List<Debuff> m_AllDebuffs;
    private List<List<int>> m_Preset = new();
    private int m_MaxPresetIndex;

    public void Initialize()
    {
        m_MaxPresetIndex = 3;
        //id自動振り分け
        for (int i = 1; i < m_AllDebuffs.Count; i++)
        {
            m_AllDebuffs[i].Id = i;
        }

        if (m_Preset.Count < m_MaxPresetIndex)
        {
            for (int i = m_Preset.Count; i < m_MaxPresetIndex; i++)
            {
                m_Preset.Add(new List<int>());
            }
        }
        if (m_Preset.Count > m_MaxPresetIndex)
        {
            for (int i = m_Preset.Count; i >= m_MaxPresetIndex; i--)
            {
                m_Preset.RemoveAt(i);
            }
        }
        Debug.Log(m_MaxPresetIndex);
        Debug.Log(m_Preset.Count);
    }

    public List<Debuff> GetDebuffsInCategory(DebuffCategory category)
    {
        List<Debuff> debuffs = new();
        foreach (Debuff debuff in m_AllDebuffs)
        {
            if (debuff.Category == category)
            {
                debuffs.Add(debuff);
            }
        }
        return debuffs;
    }

    public Debuff GetDebuff(int id)
    {
        foreach (Debuff debuff in m_AllDebuffs)
        {
            if (debuff.Id == id)
            {
                return debuff;
            }
        }
        return null;
    }

    /// <summary>
    /// プリセットに保存する関数
    /// </summary>
    /// <param name="index">保存するプリセット</param>
    /// <param name="debuffs">保存するリスト</param>
    /// <returns>正常に保存されたか</returns>
    public bool SetPreset(int index, List<int> debuffs)
    {
        if (index > m_MaxPresetIndex || debuffs.Count <= 0) return false;
        m_Preset[index] = debuffs;
        return true;
    }

    /// <summary>
    /// プリセットを取得する関数
    /// </summary>
    /// <param name="index">取得するプリセット</param>
    /// <param name="debuffs">取得したものを格納するリスト</param>
    /// <returns>正常に取得できたか</returns>
    public bool GetPreset(int index, out List<int> debuffs)
    {
        if (index > m_MaxPresetIndex || m_Preset[index].Count <= 0)
        {
            debuffs = null;
            return false;
        }
        debuffs = m_Preset[index];
        return true;
    }

    /// <summary>
    /// プリセットを削除する関数
    /// </summary>
    /// <param name="index">削除するプリセット</param>
    /// <returns>正常に削除できたか</returns>
    public bool DeletePreset(int index)
    {
        if (index > m_MaxPresetIndex || m_Preset[index].Count <= 0) return false;
        m_Preset[index].Clear();
        return true;
    }

    public List<int> PreviewPreset(int index)
    {
        if (index > m_MaxPresetIndex) return null;
        return m_Preset[index];
    }
}

using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class DebuffPreset
{
    public List<int> List;
}

[CreateAssetMenu(fileName = "DebuffData", menuName = "Scriptable Objects/DebuffData")]
public class DebuffData : ScriptableObject
{
    [SerializeField] private List<Debuff> m_AllDebuffs;
    private List<DebuffPreset> m_Preset;
    private int m_MaxPresetIndex;

    public void Initialize()
    {
        m_MaxPresetIndex = 3;
        //id自動振り分け
        for (int i = 1; i < m_AllDebuffs.Count; i++)
        {
            m_AllDebuffs[i].Id = i;
        }

        if (m_Preset == null)
        {
            m_Preset = new();
            Debug.Log("プリセットがnullなのでプリセットを初期化");
        }
        if (m_Preset.Count < m_MaxPresetIndex)
        {
            Debug.Log("プリセット枠が規定数以下なのでプリセット枠を増やす");
            for (int i = m_Preset.Count; i < m_MaxPresetIndex; i++)
            {
                m_Preset.Add(new DebuffPreset());
            }
        }
        if (m_Preset.Count > m_MaxPresetIndex)
        {
            Debug.Log("プリセット枠が規定数以上なのでプリセット枠を減らす");
            for (int i = m_Preset.Count - 1; i >= m_MaxPresetIndex; i--)
            {
                m_Preset.RemoveAt(i);
            }
        }
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
    /// <param name="index">保存するプリセットの番号</param>
    /// <param name="debuffs">保存するリスト</param>
    public void SetPreset(int index, List<int> debuffs)
    {
        if (index > m_MaxPresetIndex || debuffs.Count <= 0) return;
        m_Preset[index].List = debuffs;
    }

    /// <summary>
    /// プリセットを取得する関数
    /// </summary>
    /// <param name="index">取得するプリセットの番号</param>
    /// <returns>取得したプリセット</returns>
    public List<int> GetPreset(int index)
    {
        if (index > m_MaxPresetIndex)
        {
            return null;
        }
        return m_Preset[index].List;
    }

    /// <summary>
    /// プリセットを削除する関数
    /// </summary>
    /// <param name="index">削除するプリセットの番号</param>
    public bool DeletePreset(int index)
    {
        if (index > m_MaxPresetIndex) return false;
        m_Preset[index].List.Clear();
        return true;
    }
}
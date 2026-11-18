using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "DebuffData", menuName = "Scriptable Objects/DebuffData")]
public class DebuffData : ScriptableObject
{
    [SerializeField] private List<Debuff> m_AllDebuffs;
    private int m_InCategoryMaxCount;

    public void Initialize()
    {
        //idé©ìÆêUÇËï™ÇØ
        for (int i = 0; i < m_AllDebuffs.Count; i++)
        {
            m_AllDebuffs[i].Id = i;
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
}

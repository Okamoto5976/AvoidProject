using UnityEngine;
using System.Collections.Generic;

public class DebuffManager : MonoBehaviour
{
    [SerializeField] private List<DebuffElement> debuffs;
    [SerializeField] private List<Debuff> allDebuffs;
    private List<int> debuffList;

    public void CategoryChange(int index)
    {
        DebuffCategory category = (DebuffCategory)index;
        int listIndex = 0;
        foreach (Debuff debuff in allDebuffs)
        {
            if (debuff.Category == category)
            {
                DebuffElement element = debuffs[listIndex];
                element.SetElement(debuff, ActiveCheck(debuff.Id));
            }
        }
    }

    public void OnDebuff(int id)
    {
        if (ActiveCheck(id))
        {
            debuffList.Remove(id);
        }
        else
        {
            debuffList.Add(id);
        }
    }

    private bool ActiveCheck(int checkId)
    {
        foreach(int id in debuffList)
        {
            if (id == checkId)
            {
                return true;
            }
        }
        return false;
    }
}

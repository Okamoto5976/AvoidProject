using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DebuffPresetManager : MonoBehaviour
{
    [SerializeField] private DebuffManager m_DebuffManager;
    [SerializeField] private Button m_PreaetButtons;
    private Dictionary<string, List<int>> m_Preset = new();


}

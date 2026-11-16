using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class DebuffElement : MonoBehaviour
{
    [SerializeField] private DebuffManager m_DebuffManager;
    [SerializeField] private TextMeshProUGUI m_DebuffName;
    [SerializeField] private TextMeshProUGUI m_Reward;
    [SerializeField] private Button m_Button;
    [SerializeField] private TextMeshProUGUI m_ButtonText;

    public void SetElement(Debuff debuff, bool active)
    {
        m_DebuffName.text = debuff.Name;
        m_Reward.text = $"${debuff.Reward}";
        m_Button.onClick.RemoveAllListeners();
        SetButtonText(active);
        m_Button.onClick.AddListener(() => m_DebuffManager.OnDebuff(debuff.Id));
    }

    public void SetButtonText(bool active)
    {
        if (active)
        {
            m_ButtonText.text = "RemoveDebuff";
        }
        else
        {
            m_ButtonText.text = "AddDebuff";
        }
    }
}

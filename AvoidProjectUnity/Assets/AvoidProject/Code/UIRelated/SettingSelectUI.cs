using UnityEngine;
using UnityEngine.UI;

public class SettingSelectUI : MonoBehaviour
{
    [SerializeField] private Button m_button;

    public Button Button => m_button;

    public void SetAnim(bool value)
    {
        if (!gameObject.activeInHierarchy) return;

    }
}

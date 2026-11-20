using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingUI : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private TMP_Text m_keyText;

    public Button KeyButton => m_button;
    public TMP_Text KeyText => m_keyText;


    public void UpdateKeyText(string text)
    {
        m_keyText.text = text;
    }
}

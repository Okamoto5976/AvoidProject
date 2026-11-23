using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class KeySettingUI : MonoBehaviour
{
    [SerializeField] private Button m_button;
    [SerializeField] private TMP_Text m_keyText;
    [SerializeField] private Animator m_anim;
    public Button KeyButton => m_button;
    public TMP_Text KeyText => m_keyText;


    public void SetAnim(bool value)
    {
        if (!gameObject.activeInHierarchy) return;
        m_anim.SetBool("Selected", value);
    }

    public void UpdateKeyText(string text)
    {
        m_keyText.text = text;
    }
}

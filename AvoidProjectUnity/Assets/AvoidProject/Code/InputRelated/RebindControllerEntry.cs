using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[System.Serializable]
public class RebindControllerEntry
{
    public KeySettingUI m_button;
    //public Button m_button;
    //public TMP_Text m_keyText;
    //public Image m_controllerButtonImage;

    public Button m_keyButton => m_button.KeyButton;

    //‚Ç‚ÌInputAction‚ð•ÏX‚·‚é‚©
    public string m_actionName;

    //public Sprite m_controllerButtonSprite;
    public int m_bindingIndex;

    [System.NonSerialized]
    public InputAction m_action;

    public void UpdateKeyText(string text) => m_button.UpdateKeyText(text);
    public void UpdateButtonImage(Sprite sprite) => m_button.UpdateButtonImage(sprite);
}

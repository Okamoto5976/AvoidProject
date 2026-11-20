using TMPro;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.InputSystem;

[System.Serializable]
public class RebindControllerEntry
{
    public Button m_button;
    public TMP_Text m_keyText;
    public Image m_controllerButtonImage;

    //Ç«ÇÃInputActionÇïœçXÇ∑ÇÈÇ©
    public string m_actionName;

    public Sprite m_controllerButtonSprite;
    public int m_bindingIndex;

    [System.NonSerialized]
    public InputAction m_action;
}

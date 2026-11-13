using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


//パスをキーボードかコントローラーか
//キーボードならTextをTMPに表示
//コントローラーならSpriteをImageに表示

[System.Serializable]
public class RebindEntry
{
    public Button m_button;
    public TMP_Text m_keyText;
    public Image m_controllerButtonImage;

    //どのInputActionを変更するか
    public string m_actionName;

    public Sprite m_controllerButtonSprite;
    public int m_bindingIndex;

    [System.NonSerialized]
    public InputAction m_action;
}

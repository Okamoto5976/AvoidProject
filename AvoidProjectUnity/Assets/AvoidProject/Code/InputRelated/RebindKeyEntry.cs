using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;
using static System.Net.Mime.MediaTypeNames;


//パスをキーボードかコントローラーか
//キーボードならTextをTMPに表示
//コントローラーならSpriteをImageに表示

[System.Serializable]
public class RebindKeyEntry
{
    public KeySettingUI m_button;
    public Button m_keyButton => m_button.KeyButton;
    //public Image m_controllerButtonImage;

    //どのInputActionを変更するか
    public string m_actionName;

    //public Sprite m_controllerButtonSprite;
    public int m_bindingIndex;

    [System.NonSerialized]
    public InputAction m_action;

    public void UpdateKeyText(string text) => m_button.UpdateKeyText(text);
}

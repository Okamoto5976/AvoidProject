using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class UIManagerTest : MonoBehaviour
{
    [SerializeField] private UIManager uiManager;

    private void Update()
    {
#if ENABLE_INPUT_SYSTEM
        // êVInput System
        if (Keyboard.current == null) return;
        if (Keyboard.current.aKey.wasPressedThisFrame) uiManager.ShowPanel(UIPanelType.GameClear);
        if (Keyboard.current.sKey.wasPressedThisFrame) uiManager.ShowPanel(UIPanelType.GameOver);
#else
        // ãåInput Manager
        if (Input.GetKeyDown(KeyCode.A)) uiManager.ShowPanel(UIPanelType.GameClear);
        if (Input.GetKeyDown(KeyCode.S)) uiManager.ShowPanel(UIPanelType.GameOver);
#endif
    }
}
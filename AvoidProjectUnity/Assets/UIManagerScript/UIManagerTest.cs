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
        if (Keyboard.current == null) return;
        if (Keyboard.current.aKey.wasPressedThisFrame) uiManager.ShowPanel(UIPanelType.GameClear);
        if (Keyboard.current.sKey.wasPressedThisFrame) uiManager.ShowPanel(UIPanelType.GameOver);
#endif
    }
}
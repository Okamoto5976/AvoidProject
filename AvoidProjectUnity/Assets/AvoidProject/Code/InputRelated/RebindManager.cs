using UnityEngine;
using UnityEngine.InputSystem;

public class RebindManager : MonoBehaviour
{
    private InputActionRebindingExtensions.RebindingOperation m_rebindingOperation;

    private InputProvider m_inputProvider;

    public void Initialize(InputProvider provider)
    {
        m_inputProvider = provider;
    }

    [ContextMenu("Change Input")]
    public void TestRebind()
    {
        StartRebind(
            m_inputProvider.InputActions.Player.Attack, 0);
    }

    
    public void StartRebind(InputAction action, int bindingIndex)
    {
        if (m_rebindingOperation != null) return;

        m_rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsHavingToMatchPath("<Keyboard>")//キーボード以外の入力除外
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                //m_inputProvider.SaveRebing();

                operation.Dispose();
                m_rebindingOperation = null;
            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                m_rebindingOperation = null;
            });

        m_rebindingOperation.Start();
    }

    private void SaveRebing()
    {

    }

    private void OnDestroy()
    {
        m_rebindingOperation?.Dispose();
    }
}

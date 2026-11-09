using UnityEngine;
using UnityEngine.InputSystem;

public class RebindManager : MonoBehaviour
{
    private InputActionRebindingExtensions.RebindingOperation m_rebindingOperation;

    public void StartRebind(InputAction action, int bindingIndex)
    {
        m_rebindingOperation?.Dispose();

        m_rebindingOperation = action.PerformInteractiveRebinding(bindingIndex)
            .WithControlsHavingToMatchPath("<Keyboard>")//キーボード以外の入力除外
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                SaveRebing();

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

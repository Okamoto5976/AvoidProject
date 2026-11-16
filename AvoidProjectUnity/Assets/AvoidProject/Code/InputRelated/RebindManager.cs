using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RebindManager : MonoBehaviour
{
    [SerializeField] private List<RebindEntry> m_rebindList;


    private InputActionRebindingExtensions.RebindingOperation m_rebindingOperation;

    private InputProvider m_inputProvider;
    private InputSystem_Actions m_action;

    public void Initialize(InputProvider provider)
    {
        m_inputProvider = provider;
        m_action = m_inputProvider.InputActions;

        foreach(var entry in m_rebindList)
        {
            entry.m_action = m_action.asset.FindAction(entry.m_actionName);

            UpdateDisplay(entry);

            RebindEntry target = entry;

            entry.m_button.onClick.AddListener(
                () => StartRebind(target)
                );
        }
    }

    //[ContextMenu("Change Input")]
    //public void TestRebind()
    //{
    //    StartRebind(
    //        m_inputProvider.InputActions.Player.Attack, 1);
    //}

    [ContextMenu("Delete RebindSetting")]
    public void DeleteRebindSetting()
    {
        m_inputProvider.DeleteRebind();
    }

    private void UpdateDisplay(RebindEntry entry)
    {
        entry.m_keyText.text =
            entry.m_action.GetBindingDisplayString(entry.m_bindingIndex);
    }

    //InputAction action, int bindingIndex
    public void StartRebind(RebindEntry entry)
    {
        if (m_rebindingOperation != null) return;



        entry.m_action.Disable();

        m_rebindingOperation = entry.m_action.PerformInteractiveRebinding(entry.m_bindingIndex)
            .WithControlsHavingToMatchPath("<Keyboard>")//キーボード以外の入力除外
            .WithCancelingThrough("<Keyboard>/escape")
            .OnComplete(operation =>
            {
                operation.Dispose();
                m_rebindingOperation = null;

                entry.m_action.Enable();

                m_inputProvider.SaveRebind();

                UpdateDisplay(entry);

            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                m_rebindingOperation = null;

                entry.m_action.Enable();
            });

        m_rebindingOperation.Start();
    }

    private void OnDestroy()
    {
        m_rebindingOperation?.Dispose();
    }
}

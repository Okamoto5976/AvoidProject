using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RebindManager : MonoBehaviour
{
    [SerializeField] private List<RebindKeyEntry> m_rebindActionKeyList;
    [SerializeField] private List<RebindKeyEntry> m_rebindUIKeyList;

    [SerializeField] private List<RebindControllerEntry> m_rebindActionControllerList;
    [SerializeField] private List<RebindControllerEntry> m_rebindUIControllerList;

    [SerializeField] private GameObject m_somethingInputGuidePanel;

    private InputActionRebindingExtensions.RebindingOperation m_rebindingOperation;

    private InputProvider m_inputProvider;
    private InputSystem_Actions m_action;

    public void Initialize(InputProvider provider)
    {
        m_inputProvider = provider;
        m_action = m_inputProvider.InputActions;

        foreach(var entry in m_rebindActionKeyList)
        {
            entry.m_action = m_action.asset.FindAction(entry.m_actionName);
            //Debug.Log($"ActionName: {entry.m_actionName}");
            //Debug.Log($"取得結果: {entry.m_action}");
            UpdateDisplay(entry);

            RebindKeyEntry target = entry;

            entry.m_keyButton.onClick.AddListener(
                () => StartRebind(target)
                );
        }

        foreach (var entry in m_rebindUIKeyList)
        {
            entry.m_action = m_action.asset.FindAction(entry.m_actionName);
            //Debug.Log($"ActionName: {entry.m_actionName}");
            //Debug.Log($"取得結果: {entry.m_action}");
            UpdateDisplay(entry);

            RebindKeyEntry target = entry;

            entry.m_keyButton.onClick.AddListener(
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

    private void UpdateDisplay(RebindKeyEntry entry)
    {
        //Debug.LogWarning($"Entry Kind{entry.m_action}");

        entry.UpdateKeyText(
            entry.m_action.GetBindingDisplayString(entry.m_bindingIndex)
            );
    }

    //InputAction action, int bindingIndex
    public void StartRebind(RebindKeyEntry entry)
    {
        if (m_rebindingOperation != null) return;

        m_somethingInputGuidePanel.SetActive(true);

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
                m_somethingInputGuidePanel.SetActive(false);

            })
            .OnCancel(operation =>
            {
                operation.Dispose();
                m_rebindingOperation = null;

                entry.m_action.Enable();
                m_somethingInputGuidePanel.SetActive(false);

            });

        m_rebindingOperation.Start();
    }

    private void OnDestroy()
    {
        m_rebindingOperation?.Dispose();
    }
}

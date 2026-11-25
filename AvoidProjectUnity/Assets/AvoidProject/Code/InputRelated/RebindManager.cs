using UnityEngine;
using UnityEngine.InputSystem;
using System.Collections.Generic;

public class RebindManager : MonoBehaviour
{
    //Key Controller共通部分はInterfaceで　関数を減らす
    //Index操作は　状態　つないだコントローラーや　画面によってIndexの制限を変更して
    //使いまわす

    [SerializeField] private List<RebindKeyEntry> m_rebindActionKeyList;
    [SerializeField] private List<RebindKeyEntry> m_rebindUIKeyList;
    private int m_rebindKeyIndex;


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
            UpdateKeyDisplay(entry);

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
            UpdateKeyDisplay(entry);

            RebindKeyEntry target = entry;

            entry.m_keyButton.onClick.AddListener(
                () => StartRebind(target)
                );
        }

        foreach(var entry in m_rebindActionControllerList)
        {
            entry.m_action = m_action.asset.FindAction(entry.m_actionName);
            UpdateControllerDisplay(entry);

            RebindControllerEntry target = entry;

            entry.m_keyButton.onClick.AddListener(
                () => StartControllerRebind(target)
                );
        }
    }

    public void OpenKeyConfig()
    {
        m_rebindKeyIndex = -1;
        foreach(var entry in m_rebindActionKeyList)
        {
            entry.m_button.SetAnim(false);
        }

        foreach(var entry in m_rebindUIKeyList)
        {
            entry.m_button.SetAnim(false);
        }
    }

    public void UpdateKeyConfigSelect(Vector2 input)
    {
        if(input == Vector2.zero) return;

        if(m_rebindKeyIndex == -1)
        {
            m_rebindKeyIndex = 0;
            //Anim
            m_rebindActionKeyList[0].m_button.SetAnim(true);
            return;
        }

        int column = m_rebindKeyIndex >= 10 ? 1 : 0;
        int row = m_rebindKeyIndex >= 10
            ? m_rebindKeyIndex - 10
            : m_rebindKeyIndex;

        int oldIndex = m_rebindKeyIndex;

        if(input.x > 0)
        {
            column = 1;
        }
        else if (input.x < 0)
        {
            column = 0;
        }

        if(input.y > 0)
        {
            row--;
        }
        else if(input.y < 0)
        {
            row++;
        }

        int maxRow = column == 0 ? 9 : 3;

        row = Mathf.Clamp(row, 0, maxRow);

        m_rebindKeyIndex = column == 0
            ? row
            : row + 10;

        Debug.Log(m_rebindKeyIndex);

        if(oldIndex >= 10)
        {
            m_rebindUIKeyList[oldIndex - 10].m_button.SetAnim(false);
        }
        else
        {
            m_rebindActionKeyList[oldIndex].m_button.SetAnim(false);
        }

        if(m_rebindKeyIndex >= 10)
        {
            m_rebindUIKeyList[row].m_button.SetAnim(true);
        }
        else
        {
            m_rebindActionKeyList[row].m_button.SetAnim(true);
        }
    }

    public void DecideRebind()
    {
        RebindKeyEntry entry = null;

        if (m_rebindKeyIndex == -1) return;

        if(m_rebindKeyIndex >= 10)
        {
            entry = m_rebindUIKeyList[m_rebindKeyIndex - 10];
        }
        else
        {
            entry = m_rebindActionKeyList[m_rebindKeyIndex];

        }

        StartRebind(entry);
    }


    [ContextMenu("Delete RebindSetting")]
    public void DeleteRebindSetting()
    {
        m_inputProvider.DeleteRebind();
    }

    private void UpdateKeyDisplay(RebindKeyEntry entry)
    {
        //Debug.LogWarning($"Entry Kind{entry.m_action}");

        entry.UpdateKeyText(
            entry.m_action.GetBindingDisplayString(entry.m_bindingIndex)
            );
    }

    private void UpdateControllerDisplay(RebindControllerEntry entry)
    {
        entry.UpdateKeyText(
            entry.m_action.GetBindingDisplayString(entry.m_bindingIndex)
            );
    }

    //InputAction action, int bindingIndex
    private void StartRebind(RebindKeyEntry entry)
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

                UpdateKeyDisplay(entry);
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

    private void StartControllerRebind(RebindControllerEntry entry)
    {
        if(m_rebindingOperation != null) return;
    }

    private void OnDestroy()
    {
        m_rebindingOperation?.Dispose();
    }
}

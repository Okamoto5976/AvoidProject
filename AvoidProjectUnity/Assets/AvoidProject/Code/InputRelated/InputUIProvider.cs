using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUIProvider
{
    private InputSystem_Actions m_action;

    public InputSystem_Actions InputActions => m_action;

    //Input UI
    private Vector2 m_inputMove;

    public event Action<Vector2> OnInputMove;

    public InputUIProvider()
    {
        m_action = new InputSystem_Actions();

        m_action.UI.Navigate.performed += OnMove;
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        m_inputMove = context.ReadValue<Vector2>();

        OnInputMove?.Invoke(m_inputMove);
    }

    public void UIEnable()
    {
        m_action.UI.Enable();
    }

    public void UIDisable()
    {
        m_action.UI.Disable();
    }

}

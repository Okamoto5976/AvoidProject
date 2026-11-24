using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputUIProvider
{
    private InputSystem_Actions m_action;

    public InputSystem_Actions InputActions => m_action;

    //Input UI
    private Vector2 m_inputMove;

    private bool m_next;
    private bool m_previous;

    public event Action<Vector2> OnInputMove;

    public InputUIProvider()
    {
        m_action = new InputSystem_Actions();

        m_action.UI.Navigate.performed += OnMove;
        m_action.UI.Next.performed += OnNext;
        m_action.UI.Previous.performed += OnPrevious;
    }
    public void UIEnable()
    {
        m_action.UI.Enable();
    }

    public void UIDisable()
    {
        m_action.UI.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        m_inputMove = context.ReadValue<Vector2>();

        OnInputMove?.Invoke(m_inputMove);
    }

    private void OnNext(InputAction.CallbackContext context)
    {
        m_next = true;
    }

    private void OnPrevious(InputAction.CallbackContext context)
    {
        m_previous = true;
    }

    public bool IsNext
    {
        get
        {
            bool result = m_next;
            m_next = false;

            return result;
        }
    }

    public bool IsPrevious
    {
        get
        {
            bool result = m_previous;
            m_previous = false;

            return result;
        }
    }
}

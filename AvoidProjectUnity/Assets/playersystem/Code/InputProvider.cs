using UnityEngine;

public class InputProvider
{
    private InputSystem_Actions m_action;

    private Vector2 m_inputMove;

    public InputProvider()
    {
        m_action = new InputSystem_Actions();
        m_action.Enable();

        m_action.Player.Move.performed += OnMove;
        m_action.Player.Move.canceled += OnMove;
    }

    private void OnMove(UnityEngine.InputSystem.InputAction.CallbackContext context)
    {
        m_inputMove = context.ReadValue<Vector2>();

    }

    public Vector2 InputMove
    {
        get
        {
            return m_inputMove;
        }
    }
}

using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    private InputSystem_Actions m_action;

    private Vector2 m_inputMove;

    

    public event Action<Vector2> OnInputMove;
    public event Action OnInputInteract;
    public event Action<bool> OnInputSprint;
    public event Action OnInputAttack;
    public event Action OnInputRecover;
    public event Action OnInputAvoid;
    public event Action OnInputMenu;

    public InputProvider()
    {
        m_action = new InputSystem_Actions();
        m_action.Enable();

        m_action.Player.Move.performed += OnMove;
        m_action.Player.Move.canceled += OnMove;
        m_action.Player.Interact.performed += OnInteract;
        m_action.Player.Sprint.performed += OnSprint;
        m_action.Player.Sprint.canceled += OnSprint;
        m_action.Player.Attack.performed += OnAttack;
        m_action.Player.Recover.performed += OnRecover;
        m_action.Player.Avoid.performed += OnAvoid;
        m_action.UI.Menu.performed += OnMenu;
    }

    public void Disable()
    {
        m_action.Disable();
    }

    private void OnMove(InputAction.CallbackContext context)
    {
        m_inputMove = context.ReadValue<Vector2>();

        OnInputMove?.Invoke(m_inputMove);
    }

    private void OnInteract(InputAction.CallbackContext context)
    {
        OnInputInteract?.Invoke();
    }
    private void OnSprint(InputAction.CallbackContext context)
    {
        OnInputSprint?.Invoke(context.performed);
    }
    private void OnAttack(InputAction.CallbackContext context)
    {
        OnInputAttack?.Invoke();
    }
    private void OnRecover(InputAction.CallbackContext context)
    {
        OnInputRecover?.Invoke();
    }
    private void OnAvoid(InputAction.CallbackContext context)
    {
        OnInputAvoid?.Invoke();
    }   

    private void OnMenu(InputAction.CallbackContext context)
    {
        OnInputMenu?.Invoke();
    }
}

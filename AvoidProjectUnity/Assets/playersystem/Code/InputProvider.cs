using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class InputProvider
{
    private const string REBIND_KEY = "Player_Rebinds";

    private InputSystem_Actions m_action;

    public InputSystem_Actions InputActions => m_action;

    //Input Player
    private Vector2 m_inputMove;
    public event Action<Vector2> OnInputMove;
    public event System.Action OnInputInteract;
    public event Action<bool> OnInputSprint;
    public event System.Action OnInputAttack;
    public event System.Action OnInputRecover;
    public event System.Action OnInputAvoid;
    public event Action<bool> OnInputInteractHold;
    public event Action<bool> OnInputRecoverHold;

    //Input UI
    public event System.Action OnInputMenu;

    public InputProvider()
    {
        m_action = new InputSystem_Actions();

        LoadRebind();


        m_action.Player.Move.performed += OnMove;
        m_action.Player.Move.canceled += OnMove;
        m_action.Player.Interact.performed += OnInteract;
        m_action.Player.Interact.started += OnInteractHold;
        m_action.Player.Interact.canceled += OnInteractHold;
        m_action.Player.Sprint.performed += OnSprint;
        m_action.Player.Sprint.canceled += OnSprint;
        m_action.Player.Attack.performed += OnAttack;
        m_action.Player.Recover.performed += OnRecover;
        m_action.Player.Recover.started += OnRecoverHold;
        m_action.Player.Recover.canceled += OnRecoverHold;
        m_action.Player.Avoid.performed += OnAvoid;


        m_action.UI.Menu.performed += OnMenu;

    }

    public void PlayerEnable()
    {
        m_action.Player.Enable();
    }

    public void PlayerDisable()
    {
        m_action.Player.Disable();
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
    private void OnInteractHold(InputAction.CallbackContext context)
    {
        OnInputInteractHold?.Invoke(context.started);
    }
    private void OnRecoverHold(InputAction.CallbackContext context)
    {
        OnInputRecoverHold?.Invoke(context.started);
    }   

    private void OnMenu(InputAction.CallbackContext context)
    {
        OnInputMenu?.Invoke();
    }

    public void SaveRebind()
    {
        string json = m_action.SaveBindingOverridesAsJson();

        PlayerPrefs.SetString(REBIND_KEY, json);
        PlayerPrefs.Save();
    }

    private void LoadRebind()
    {
        if (!PlayerPrefs.HasKey(REBIND_KEY))
            return;

        string json = PlayerPrefs.GetString(REBIND_KEY);

        m_action.LoadBindingOverridesFromJson(json);
    }

    public void DeleteRebind()
    {
        PlayerPrefs.DeleteKey(REBIND_KEY);
        PlayerPrefs.Save();

    }
}

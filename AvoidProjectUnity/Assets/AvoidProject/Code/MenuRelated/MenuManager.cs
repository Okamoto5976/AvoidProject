using UnityEngine;

public class MenuManager : MonoBehaviour
{
    //ReferenceObject
    [Header("Necessary Reference")]
    [SerializeField] private GameObject m_menu;
    [SerializeField] private GameObject m_option;

    //InputController

    [Header("Not Necessary Reference")]
    private GameManager m_gameManager;


    //[SerializeField] private 
    //GoreController

    //InputProvider
    private InputProvider m_inputProvider;

    private void OnDisable()
    {
        if(m_inputProvider == null) return;

        m_inputProvider.OnInputMenu -= InputMenu;
        m_inputProvider.UIDisable();
    }

    private void OnEnable()
    {
        if(m_inputProvider == null) return;

        m_inputProvider.OnInputMenu += InputMenu;
        m_inputProvider.UIEnable();
    }

    public void Initialize(InputProvider inputProvider)
    {
        //OptionSaveÇ©ÇÁåªç›ê›íËÇ≥ÇÍÇƒÇ¢ÇÈVolumeÇMixerÇ∆SliderÇ…ê›íË

        m_inputProvider = inputProvider;
        m_inputProvider.OnInputMenu += InputMenu;
    }

    public void InputMenu()
    {
        if (m_gameManager.GameState == GameManager.EnumGameState.Event ||
            m_gameManager.GameState == GameManager.EnumGameState.GameOver) return;

        if(m_gameManager.GameState == GameManager.EnumGameState.InPlay)
        {
            OnOpenMenu();

        }
        else if(m_gameManager.GameState == GameManager.EnumGameState.Pouse)
        {
            OnCloseMenu();
            OnCloseOption();
        }

    }

    public void OnOpenMenu()
    {
        m_menu.SetActive(true);
        //GameManager State Pouse
        m_gameManager.ChangeGameState(GameManager.EnumGameState.Pouse);
    }

    public void OnCloseMenu()
    {
        m_menu.SetActive(false);
        //GameManager State InPlay
        m_gameManager.ChangeGameState(GameManager.EnumGameState.InPlay);

    }

    public void OnOpenOption()
    {
        m_option.SetActive(true);
    }

    public void OnCloseOption()
    {
        m_option.SetActive(false);
    }

    public void SetGameManager(GameManager component)
    {
        m_gameManager = component;
    }
}

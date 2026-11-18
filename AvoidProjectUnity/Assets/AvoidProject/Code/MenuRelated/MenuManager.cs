using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class MenuManager : MonoBehaviour
{
    public enum MenuState
    { 
        None,
        Menu,
        Option
    }

    //ReferenceObject
    [Header("Necessary Reference")]
    [SerializeField] private GameObject m_menuPanel;
    [SerializeField] private GameObject m_optionPanel;

    [Header("Menu Buttons")]
    [SerializeField] private MenuTextUI m_menuContinuButton;
    [SerializeField] private MenuTextUI m_menuOptionButton;
    [SerializeField] private MenuTextUI m_menuTitleButton;

    [Header("Option Buttons")]
    [SerializeField] private GameObject m_option;
    [SerializeField] private GameObject m_keyConfig;
    [SerializeField] private GameObject m_controllerConfig;
    [SerializeField] private OptionSelectUI m_optionButton;
    [SerializeField] private OptionSelectUI m_keyConfigButton;
    [SerializeField] private OptionSelectUI m_controllerConfigButton;

    [Header("Not Necessary Reference")]
    private List<MenuTextUI> m_menuButtons = new();
    private int m_menuButtonsIndex = 0;

    private GameManager m_gameManager;

    private MenuState m_menuState = MenuState.None;
    //[SerializeField] private 
    //GoreController


    //InputProvider
    private InputProvider m_inputProvider;

    private InputUIProvider m_inputUiProvider;

    private void OnDisable()
    {
        if(m_inputProvider != null)
        {
            m_inputProvider.OnInputMenu -= OnInputMenu;
            m_inputProvider.UIDisable();
        }

        if(m_inputUiProvider != null)
        {
            m_inputUiProvider.OnInputMove -= OnInputMove;
            m_inputUiProvider.UIDisable();
        }
    }

    private void OnEnable()
    {
        if(m_inputProvider != null)
        {
            m_inputProvider.OnInputMenu += OnInputMenu;
        }

        if( m_inputUiProvider != null)
        {
            m_inputUiProvider.OnInputMove += OnInputMove;
        }
    }

    private void Awake()
    {
        m_inputUiProvider = new InputUIProvider();

        m_menuButtons.Add(m_menuTitleButton);//index0
        m_menuButtons.Add(m_menuOptionButton);//index1
        m_menuButtons.Add(m_menuContinuButton);//index2

        //Menu画面のボタンを設定
        m_menuContinuButton.Button.onClick.AddListener(() => OnSelectContinu());
        m_menuOptionButton.Button.onClick.AddListener(() => OnSelectOption());
        m_menuTitleButton.Button.onClick.AddListener(() => OnSelectTitle());

        //Option画面上部のボタン設定
        m_optionButton.Button.onClick.AddListener(() => OnChangeOption());
        m_keyConfigButton.Button.onClick.AddListener(() => OnChangeKeyConfig());
        m_controllerConfigButton.Button.onClick.AddListener(() => OnChangeControllerConfig());
    }

    public void Initialize(InputProvider inputProvider)
    {
        //OptionSaveから現在設定されているVolumeをMixerとSliderに設定

        m_inputProvider = inputProvider;
        m_inputProvider.OnInputMenu += OnInputMenu;


    }
    public void SetGameManager(GameManager component)
    {
        m_gameManager = component;
    }

    public void OnInputMenu()
    {
        if (m_gameManager.GameState == GameManager.EnumGameState.Event ||
            m_gameManager.GameState == GameManager.EnumGameState.GameOver) return;

        if(m_menuState == MenuState.Option)
        {
            m_optionPanel.SetActive(false);
            m_menuPanel.SetActive(true);

            m_menuButtonsIndex = 1;
            m_menuButtons[m_menuButtonsIndex].SetAnim(true);

            ChangeMenuState(MenuState.Menu);

            return;
        }

        if(m_gameManager.GameState == GameManager.EnumGameState.InPlay)
        {
            OpenMenu();
            m_gameManager.ChangeGameState(GameManager.EnumGameState.Pouse);

            m_menuButtonsIndex = 2;
            m_menuButtons[m_menuButtonsIndex].SetAnim(true);
            m_inputProvider.UIEnable();
            m_inputUiProvider.UIEnable();

            ChangeMenuState(MenuState.Menu);

        }
        else if(m_gameManager.GameState == GameManager.EnumGameState.Pouse)
        {
            CloseMenu();
            m_gameManager.ChangeGameState(GameManager.EnumGameState.InPlay);

            ChangeMenuState(MenuState.None);
            m_inputProvider.UIDisable();
            m_inputUiProvider.UIDisable();


        }

    }


    private void Update()
    {
        if(m_inputProvider.IsSubmit)
        {
            m_menuButtons[m_menuButtonsIndex].Button.onClick.Invoke();
        }
    }

    //十字キーで操作　コントローラーなら左スティック入力で　画面にカーソルを表示
    //矢印キーで操作（asdwでもいい）

    private void OnInputMove(Vector2 input)
    {
        //foreach(var b in m_menuButtons)
        //{
        //    b.SetAnim(false);
        //}

        //int x;
        int y = 0;
        //if (input.x > 0)
        //{
        //    x = 1;
        //}
        //else if (input.x > 0)
        //{
        //    x = -1;
        //}

        if(input.y > 0)
        {
            y = 1;
        }
        else if(input.y < 0)
        {
            y = -1;
        }
        else
        {
            //入力がない
            return;
        }

        var index = m_menuButtonsIndex;

        m_menuButtonsIndex += y;

        if(m_menuButtonsIndex > m_menuButtons.Count - 1)
        {
            m_menuButtonsIndex = 0;
        }
        else if(m_menuButtonsIndex < 0)
        {
            m_menuButtonsIndex = m_menuButtons.Count - 1;
        }

        m_menuButtons[index].SetAnim(false);
        m_menuButtons[m_menuButtonsIndex].SetAnim(true);
    }

    private void ChangeMenuState(MenuState state)
    {
        if(m_menuState == state) return;
        m_menuState = state;
    }

    #region Menu
    public void OpenMenu() => m_menuPanel.SetActive(true);
    public void CloseMenu() => m_menuPanel.SetActive(false);

    //button.onClick.Invoke();
    public void OnSelectContinu()
    {
        CloseMenu();
        m_gameManager.ChangeGameState(GameManager.EnumGameState.InPlay);

        ChangeMenuState(MenuState.None);
        m_inputProvider.UIDisable();
        m_inputUiProvider.UIDisable();

    }

    public void OnSelectOption()
    {
        m_optionPanel.SetActive(true);
        ChangeMenuState(MenuState.Option);

        CloseMenu();
    }

    public void OnSelectTitle()
    {
        Debug.Log("Come back to Title", this);
    }

    #endregion

    #region Option
    //オプション画面上部切り替えEvent
    public void OnChangeOption()
    {
        CloseKeyConfig();
        CloseControllerConfig();
        OpenOption();
    }

    public void OnChangeKeyConfig()
    {
        CloseOption();
        CloseControllerConfig();
        OpenKeyConfig();
    }

    public void OnChangeControllerConfig()
    {
        CloseOption();
        CloseKeyConfig();
        OpenControllerConfig();
    }

    private void OpenOption() => m_option.SetActive(true);
    private void CloseOption() => m_option.SetActive(false);
    private void OpenKeyConfig() => m_keyConfig.SetActive(true);
    private void CloseKeyConfig() => m_keyConfig.SetActive(false);
    public void OpenControllerConfig() => m_controllerConfig.SetActive(true);
    public void CloseControllerConfig() => m_controllerConfig.SetActive(false);
    #endregion

}

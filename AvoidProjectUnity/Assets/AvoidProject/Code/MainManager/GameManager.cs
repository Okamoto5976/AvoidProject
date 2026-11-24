using System.Collections;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum EnumGameState
    {
        InPlay,
        Pouse,
        Event,
        GameOver,
    }

    public EnumGameState GameState => m_gameState;
    private EnumGameState m_gameState = EnumGameState.Event;

    //Propaty
    public bool HasTimeLimit => m_hasTimeLimit;
    public float TimeLimit => m_timeLimit;

    private bool m_hasTimeLimit;

    private float m_timeLimit;

    private UIManager m_uiManager;

    private InputProvider m_inputProvider;

    //SlowMotion Variable
    [SerializeField] private float m_slowScale = 0.3f;
    [SerializeField] private float m_slowDuration = 0.5f;
    [SerializeField] private float m_recoveryDuration = 0.5f;
    [SerializeField] private AnimationCurve m_recoveryCurve = AnimationCurve.EaseInOut(0f, 0f, 1f, 1f);
    private Coroutine m_slowCoroutine;

    /// <summary>
    /// 初期化時に呼ぶ　
    /// </summary>
    /// <param name="timer"></param>
    public void Initialize(
        UIManager uiMa,
        InputProvider inputProvider,
        bool hasTimeLimit,
        float timer = 0)//後々引数が増えるようなら　別ごとに関数を作るか　Classで渡してあげる
    {
        m_uiManager = uiMa;
        m_inputProvider = inputProvider;
        m_hasTimeLimit = hasTimeLimit;
        m_timeLimit = timer;
    }
    
    public void StartGame()
    {
        ChangeGameState(EnumGameState.InPlay);
    }

    private void Update()
    {
        if(m_gameState == EnumGameState.Pouse)
        {
            Time.timeScale = 0f;
        }
        else
        {
            Time.timeScale = 1f;
        }

        if (m_gameState != EnumGameState.InPlay) return;

        InPlayUpdate();
    }

    private void InPlayUpdate()
    {
        if(m_hasTimeLimit)
        {
            m_timeLimit -= Time.deltaTime;

            if (m_timeLimit < 0)
            {
                GameClearEvent();
            }
        }
    }

    public void SlowMotionEvent()
    {
        if (m_slowCoroutine != null)
        {
            StopCoroutine(m_slowCoroutine);
            m_slowCoroutine = null;
        }

        m_slowCoroutine = StartCoroutine(SlowMotionCo());
    }

    private IEnumerator SlowMotionCo()
    {
        Time.timeScale = m_slowScale;
        yield return new WaitForSecondsRealtime(m_slowDuration);
        float elapsed = 0f;

        while (elapsed < m_recoveryDuration)
        {
            elapsed += Time.unscaledDeltaTime;
            float rate = Mathf.Clamp01(elapsed / m_recoveryDuration);
            float curveValue = m_recoveryCurve.Evaluate(rate);
            Time.timeScale = Mathf.Lerp(m_slowScale, 1f , curveValue);
        }
        yield return null;
        Time.timeScale = 1f;
        m_slowCoroutine = null;
    }


    public void ChangeGameState(EnumGameState state)
    {
        if(m_gameState == state) return;

        m_gameState = state;
    }

    //Time.Scaleの操作
    //時間制限の操作
    //ステージタイプから時間制限か　討伐か
    //時間制限ならGameManagerで
    //討伐なら　討伐後にEventを走らせる（時間制限でも討伐はできるので　デフォルト）

    public void OnEnemyDead()
    {
        //GameClear（Event）時に2重になるのを防ぐ
        if (m_gameState != EnumGameState.InPlay) return;

        GameClearEvent();
    }

    public void OnGameOverEvent()
    {
        if (m_gameState == EnumGameState.GameOver) return;

        ChangeGameState(EnumGameState.GameOver);
        m_inputProvider.PlayerDisable();
        if(m_slowCoroutine != null)
        {
            StopCoroutine(m_slowCoroutine);
            m_slowCoroutine = null;
        }
        
        m_uiManager.ShowGameOver();
    }

    public void GameClearEvent()
    {
        ChangeGameState(EnumGameState.Event);
        m_inputProvider.PlayerDisable();

        if (m_slowCoroutine != null)
        {
            StopCoroutine(m_slowCoroutine);
            m_slowCoroutine = null;
        }
        Time.timeScale = 0f;
        m_uiManager.ShowGameClear();

    }



}

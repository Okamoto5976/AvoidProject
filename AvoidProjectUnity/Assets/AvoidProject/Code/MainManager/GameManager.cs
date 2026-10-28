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


    /// <summary>
    /// 初期化時に呼ぶ　
    /// </summary>
    /// <param name="timer"></param>
    public void Initialize(bool hasTimeLimit, float timer = 0)//後々引数が増えるようなら　別ごとに関数を作るか　Classで渡してあげる
    {
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

    public void GameOverEvent()
    {

    }

    public void GameClearEvent()
    {

    }


}

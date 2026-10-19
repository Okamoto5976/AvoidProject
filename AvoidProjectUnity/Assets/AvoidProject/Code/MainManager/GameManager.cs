using UnityEngine;

public class GameManager : MonoBehaviour
{
    public enum GameState
    {
        InPlay,
        Pouse,
        Event,
        GameOver,
    }

    private GameState m_gameState = GameState.Event;

    private float m_gameTimer;


    public void Initialized(float timer)//タイプも
    {
        m_gameTimer = timer;
    }

    private void Update()
    {
        m_gameTimer -= Time.deltaTime;

        if(m_gameTimer < 0)
        {
            GameClearEvent();
        }
    }

    //Time.Scaleの操作
    //時間制限の操作
    //ステージタイプから時間制限か　討伐か
    //時間制限ならGameManagerで
    //討伐なら　討伐後にEventを走らせる（時間制限でも討伐はできるので　デフォルト）

    public void GameOverEvent()
    {

    }

    public void GameClearEvent()
    {

    }
}

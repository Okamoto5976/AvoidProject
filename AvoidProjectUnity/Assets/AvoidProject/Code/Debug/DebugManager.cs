using UnityEngine;

public class DebugManager : MonoBehaviour
{
    //TimeLimit表記(UIで表記するから　HasTimeLimitのみでいい）
    //Playerの無敵化
    //EnemyのHPの変更
    //時間を遅く
    //必要な値をTextで表示　（InspecterのDebugModeをしなくていいように
    //表示するTextの選択

    //Playerの値を上書き(本来Saveから降ろすのを上書きなので　Saveに影響はないように　ゲーム終了後セーブをしない）
    //Resetも持たせる
    //現在の敵の値変更
    //敵の変更 + シーンリセット
    //Eventスキップ
    //このシーンで敵の挙動を確認できる仕組みを組み込む

    //DebugMode　管理しやすい中身を考えているが　これだけでなく　他の人がわかりやすく　調整のことを考える必要あり
    //Playerの調整などが 一番あてはまるだろう

    [SerializeField] private bool m_isDebugMode;


    private GameManager m_gameManager;

    private HpController m_playerHpController;

    public void SetGameManager(GameManager manager)
    {
        m_gameManager = manager;
    }

    public void SetPlayerHPController(HpController hpController)
    {
        m_playerHpController = hpController;
    }

    private void OnGUI()
    {
        if (!m_isDebugMode) return;

        if(m_gameManager.HasTimeLimit)
        {
            GUI.Label(
            new Rect(10, 10, 200, 30),
            $"TimeLimit :" + m_gameManager.TimeLimit.ToString("F0")
            );
        }
        else
        {
            GUI.Label(
            new Rect(10, 10, 200, 30),
            $"No Has TimeLimit"
            );
        }

        if(m_playerHpController.Hp > 0)
        {
            GUI.Label(
                new Rect(10, 40, 200, 30),
                $"PlayerHP :" + m_playerHpController.Hp);
        }
        else
        {
            GUI.Label(
                new Rect(10, 40, 200, 30),
                $"PlayerHP :" + 0);
        }

        
    }

}

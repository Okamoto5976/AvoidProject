using TMPro;
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


    [SerializeField] private bool m_isDebugMode;


    private GameManager m_gameManager;
    private string m_timeLimitText;

    //UIで表示する
    [Header("UI Reference")]
    [SerializeField] private Canvas m_canvas;
    [SerializeField] private TMP_Text m_debugText;

    public void Initialized(GameManager manager)
    {
        m_gameManager = manager;
    }

    private void Update()
    {
        if (!m_isDebugMode)
        {
            m_canvas.enabled = false;
            return;
        }
        else
        {
            m_canvas.enabled = true;

        }

        UpdateTimeLimitText();

        m_debugText.text =
            m_timeLimitText + "\n";


    }

    private void UpdateTimeLimitText()
    {
        if (m_gameManager.HasTimeLimit)
        {
            m_timeLimitText = "TimeLimit" + m_gameManager.TimeLimit.ToString("F0");
        }
        else
        {
            m_timeLimitText = "";
        }
    }
}

using UnityEngine;

public class InitializedManager : MonoBehaviour
{
    //InitializedValue
    [Header("InitializedValue : Test")]

    [SerializeField] private bool m_hasTimeLimit;
    [SerializeField] private float m_timeLimit;

    //GameObjectReference
    [Header("GameObjectReference")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private StageManager m_stageManager;

    [SerializeField] private Movement m_playerMovement;

    [SerializeField] private DebugManager m_debugManager;

    private void Awake()
    {
        Initialized();
    }

    private void Initialized()//Event設定があるなら　解除の設定もここで？
    {
        //RuntimeDataを降ろす

        //アルファ版
        //HasTimeLimit
        //TimeLimit
        //回復薬
        
        //PlayerData(SO)

        //GameTypeを受け取る　制限時間か　討伐か
        //GameManagerの初期化
        m_gameManager.Initialized(m_hasTimeLimit, m_timeLimit);

        //StageManager初期化
        m_stageManager.Initialized(5f);



        //Playerの生成-------------------------
        //RunTimeDataも渡す
        //PlayerのAwake（初期化）を呼ぶ
        //PlayerのDisable
        m_playerMovement.Initialized(m_stageManager);


        //Enemyの生成--------------------------
        //Runtimeから降ろした敵の生成
        //敵の初期化（RunTimeのものを初期化）
        //EnemyのDisable

        //それぞれPoolがあるので　あったPoolを各Managerに渡す

        //それぞれに　必要なManagerを渡す Attack時に必要なものとか

        //UIManagerのEventをつける

        //DebugManager設定
        m_debugManager.Initialized(m_gameManager);

        //GameStartDirectorを呼ぶ

        //仮でGameStart
        m_gameManager.StartGame();
    }

    //討伐時にクリア判定をする必要がある　EnemyManagerが敵の死亡を確認するべき
    //そしてGameManagerに通知する
}

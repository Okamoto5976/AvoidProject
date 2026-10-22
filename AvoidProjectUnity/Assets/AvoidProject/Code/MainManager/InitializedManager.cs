using UnityEngine;

public class InitializedManager : MonoBehaviour
{
    //InitializedValue
    [Header("InitializedValue : Test")]

    //GameManager
    [SerializeField] private bool m_hasTimeLimit;
    [SerializeField] private float m_timeLimit;

    //StartManager
    [SerializeField] private float m_stageRadius = 5f;

    //GameObjectReference
    [Header("GameObjectReference")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private StageManager m_stageManager;

    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_enemy;


    [SerializeField] private CameraManager m_cameraManager;

    [SerializeField] private Movement m_playerMovement;


    [SerializeField] private DebugManager m_debugManager;

    private void Start()
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
        m_stageManager.Initialized(m_stageRadius);



        //Playerの生成-------------------------
        //RunTimeDataも渡す
        //PlayerのAwake（初期化）を呼ぶ
        //PlayerのDisable
        m_playerMovement.Initialized(m_stageManager);


        //Enemyの生成--------------------------
        //Runtimeから降ろした敵の生成
        //敵の初期化（RunTimeのものを初期化）
        //EnemyのDisable

        //Cameraの初期化
        m_cameraManager.Initialized(m_player, m_enemy);

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

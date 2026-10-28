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
    [SerializeField] private MenuManager m_menuManager;
    [SerializeField] private StageManager m_stageManager;

    [SerializeField] private GameObject m_player;
    [SerializeField] private GameObject m_enemy;


    [SerializeField] private CameraManager m_cameraManager;

    [SerializeField] private Movement m_playerMovement;


    [SerializeField] private DebugManager m_debugManager;

    private void Start()
    {
        Initialize();
    }

    private void Initialize()//Event設定があるなら　解除の設定もここで？
    {
        //RuntimeDataを降ろす

        //アルファ版
        //HasTimeLimit
        //TimeLimit
        //回復薬
        
        //PlayerData(SO)

        //GameTypeを受け取る　制限時間か　討伐か
        //GameManagerの初期化
        m_gameManager.Initialize(m_hasTimeLimit, m_timeLimit);

        //MenuManagerの初期化
        m_menuManager.Initialized();
        m_menuManager.SetGameManager(m_gameManager);

        //StageManager初期化
        m_stageManager.Initialize(m_stageRadius);



        //Playerの生成-------------------------
        //RunTimeDataも渡す
        //PlayerのAwake（初期化）を呼ぶ
        //PlayerのDisable
        m_playerMovement.Initialize(m_stageManager);


        //Enemyの生成--------------------------
        //Runtimeから降ろした敵の生成
        //敵の初期化（RunTimeのものを初期化）
        //EnemyのDisable

        //Cameraの初期化
        m_cameraManager.Initialize(m_player, m_enemy);

        //それぞれPoolがあるので　あったPoolを各Managerに渡す

        //それぞれに　必要なManagerを渡す Attack時に必要なものとか

        //UIManagerのEventをつける

        //DebugManager設定
        m_debugManager.Initialize(m_gameManager);

        //GameStartDirectorを呼ぶ

        //仮でGameStart
        m_gameManager.StartGame();
    }

    //討伐時にクリア判定をする必要がある　EnemyManagerが敵の死亡を確認するべき
    //そしてGameManagerに通知する
}

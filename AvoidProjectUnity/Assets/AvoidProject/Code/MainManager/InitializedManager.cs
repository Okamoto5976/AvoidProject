using UnityEngine;

public class InitializedManager : MonoBehaviour
{
    //InitializedValue
    [Header("InitializedValue : Test")]

    //GameManager
    [SerializeField] private float m_timeLimit;

    //StartManager
    [SerializeField] private float m_stageRadius = 5f;

    [SerializeField] private int m_stageWeaponCount = 4;

    [SerializeField] private RuntimeData m_runtimeData = new();

    //GameObjectReference
    [Header("GameObjectReference")]
    [SerializeField] private GameManager m_gameManager;
    [SerializeField] private MenuManager m_menuManager;
    [SerializeField] private AudioVolumeController m_audioVolumeController;
    [SerializeField] private StageManager m_stageManager;

    [SerializeField] private PlayerData m_playerData;
    [SerializeField] private GameObject m_player;
    private PlayerController m_playerController;
    private HpController m_playerHpController;

    [SerializeField] private Enemy m_enemy;
    [SerializeField] private EnemyData m_enemyData;

    [SerializeField] private AttackManager m_attackManager;
    [SerializeField] private AttackViewManager m_attackViewManager;
    [SerializeField] private ColliderManager m_colliderManager;
    [SerializeField] private ColliderMediationPool m_colliderMediationPool;
    [SerializeField] private PoolManager m_attackViewPool;
     
    [SerializeField] private CameraManager m_cameraManager;




    [SerializeField] private DebugManager m_debugManager;

    

    private void Start()
    {
        Application.targetFrameRate = 60;

        Initialize();
    }

    //別のスクリプトを渡すとき　そのスクリプトは初期化を終えてるかを見る
    //GameManagerを初期化後に　StageManagerに渡すような

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
        bool hasTimeLimit = m_runtimeData.HasTimeLimit;
        m_gameManager.Initialize(hasTimeLimit, m_timeLimit);

        //MenuManagerの初期化
        m_menuManager.Initialize();
        m_menuManager.SetGameManager(m_gameManager);

        m_audioVolumeController.Initialize();

        //StageManager初期化
        m_stageManager.Initialize(m_gameManager);
        m_stageManager.StageRadiusInitialize(m_stageRadius);
        m_stageManager.StageWeaponInitialize(m_stageWeaponCount);



        //Playerの生成-------------------------
        //RunTimeDataも渡す
        //PlayerのAwake（初期化）を呼ぶ
        //PlayerのDisable
        m_playerController = m_player.GetComponent<PlayerController>();
        m_playerHpController = m_player.GetComponent<HpController>();

        m_playerController.Initialize(m_playerData, m_runtimeData, m_stageManager);
        m_playerController.SetAttackManager(m_attackManager);
        //Deadの際のイベントを入れておく


        //Enemyの生成--------------------------
        m_enemy.InitializeData(m_enemyData);

        //Runtimeから降ろした敵の生成
        //敵の初期化（RunTimeのものを初期化）
        //EnemyのDisable

        //Cameraの初期化
        m_cameraManager.Initialize(m_player, m_enemy.gameObject);

        //それぞれPoolがあるので　あったPoolを各Managerに渡す
        m_attackManager.SetAttackViewManager(m_attackViewManager);
        m_attackManager.SetColliderManager(m_colliderManager);

        m_attackViewManager.SetAttackManager(m_attackManager);
        m_attackViewManager.SetAttackViewPool(m_attackViewPool);

        m_colliderManager.SetColliderMediationPool(m_colliderMediationPool);

        //それぞれに　必要なManagerを渡す Attack時に必要なものとか

        //UIManagerのEventをつける

        //DebugManager設定
        m_debugManager.SetGameManager(m_gameManager);
        m_debugManager.SetPlayerHPController(m_playerHpController);

        //GameStartDirectorを呼ぶ


        //------------以下　GameStartDirector ---------------------
        //仮でGameStart
        m_stageManager.GenerateStageWeapon();

        m_gameManager.StartGame();
        
    }

    //討伐時にクリア判定をする必要がある　EnemyManagerが敵の死亡を確認するべき
    //そしてGameManagerに通知する
}

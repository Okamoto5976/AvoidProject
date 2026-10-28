using UnityEngine;
using System.Collections.Generic;

public class StageManager : MonoBehaviour
{
    [SerializeField] private Vector2 m_center;
    private float m_limitRadius;


    private GameManager m_gameManager;

    //StageWeapon Related
    [SerializeField] private GameObject m_weaponSwordPrefab;

    private int m_weaponCount;
    private List<GameObject> m_weaponSwordList;

    [SerializeField] private float m_regenerateWeaponTimeValue;
    private float m_regenerateWeaponTimer;

    public void Initialize(GameManager component)
    {
        m_gameManager = component;
    }

    /// <summary>
    /// ステージの大きさ初期化時
    /// </summary>
    /// <param name="radius"></param>
    public void StageRadiusInitialize(float radius)//後々引数が増えるようなら　別ごとに関数を作るか　Classで渡してあげる
    {
        m_limitRadius = radius;
    }

    /// <summary>
    /// ステージの武器生成の初期化
    /// </summary>
    /// <param name="weponCount"></param>
    public void StageWeaponInitialize(int weponCount)
    {
        m_weaponCount = weponCount;
        m_weaponSwordList = new();

        for(int i = 0; i < m_weaponCount; i++)
        {
            var obj = Instantiate(m_weaponSwordPrefab, this.transform);

            var component = obj.GetComponent<WeaponSword>();
            component.Initialize(this);

            obj.SetActive(false);

            m_weaponSwordList.Add(obj);
        }
    }

    private void Update()
    {
        //剣がいくつあるか確認する
        //一つ減ったとき　時間を図り　再生成する
        //数が決まってるのでPoolを作らず　このManagerないで管理する
        //剣にReturnStageManagerを作る

        if(m_gameManager.GameState == GameManager.EnumGameState.InPlay)
        {
            UpdateInPlay();
        }
    }

    private void UpdateInPlay()
    {
        //ステージ上の武器が戻ってきている
        if(m_weaponSwordList.Count != 0)
        {
            if(m_regenerateWeaponTimer > m_regenerateWeaponTimeValue)
            {
                m_regenerateWeaponTimer = 0f;
                var obj = GetWeaponSword();

                if (obj == null)
                {
                    Debug.LogWarning("Not Found WeaponSword in List", this);
                    return;
                }

                Vector2 randomPos = Random.insideUnitCircle * m_limitRadius;

                obj.transform.position = randomPos;

                obj.SetActive(true);
            }

            m_regenerateWeaponTimer += Time.deltaTime;
        }
    }

    /// <summary>
    /// ステージの武器生成の開始（ゲーム開始時）
    /// </summary>
    public void GenerateStageWeapon()
    {
        for(int i = 0;i < m_weaponCount;i++)
        {
            var obj = GetWeaponSword();

            if(obj == null)
            {
                Debug.LogWarning("Not Found WeaponSword in List", this);
                return;
            }

            Vector2 randomPos = Random.insideUnitCircle * m_limitRadius;

            obj.transform.position = randomPos;

            obj.SetActive(true);
        }
    }

    private GameObject GetWeaponSword()
    {

        foreach (var obj in m_weaponSwordList)
        {
            if (m_weaponSwordList.Count != 0)
            {
                m_weaponSwordList.Remove(obj);

                return obj;
            }
        }

        return null;
    }

    /// <summary>
    /// ステージ上のWeaponSwordをListに戻す
    /// </summary>
    /// <param name="obj"></param>
    public void ReturnWeaponSword(GameObject obj)
    {
        obj.SetActive(false);
        obj.transform.position = m_center;
        m_weaponSwordList.Add(obj);
    }

    /// <summary>
    /// ステージ制限内にPosをとどめる
    /// </summary>
    /// <param name="pos"></param>
    /// <returns></returns>
    public Vector2 LimitMovement(Vector2 pos)
    {
        Vector2 offset = pos - m_center;

        if(offset.magnitude > m_limitRadius)
        {
            pos = m_center + offset.normalized * m_limitRadius;
        }

        return pos;
    }
}

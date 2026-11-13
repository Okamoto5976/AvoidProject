using UnityEngine;

/// <summary>
/// 敵の生成と配置を担当するマネージャー
/// 今は指定位置にPrefabを生成するだけ
/// </summary>
public class EnemyManager : MonoBehaviour
{
    [Header("生成設定")]
    [SerializeField] private GameObject m_enemyPrefab;
    [SerializeField] private Transform m_spawnPoint;

    private void Start()
    {
        if (m_enemyPrefab == null || m_spawnPoint == null)
        {
            Debug.LogWarning("EnemyManager: m_enemyPrefab または m_spawnPoint が未設定です。");
            return;
        }

        SpawnEnemy(m_spawnPoint.position);
    }

    /// <summary>
    /// 指定した位置に敵Prefabを生成して配置する。
    /// </summary>
    public GameObject SpawnEnemy(Vector3 pos)
    {
        GameObject enemy = Instantiate(m_enemyPrefab, pos, Quaternion.identity);

        //将来ここで敵の初期化処理を呼ぶ

        return enemy;
    }
}
using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR
using UnityEditor;
#endif

[System.Serializable]
public class EnemyAttackSetting
{
    [SerializeField] private string m_attackPointId;

    [SerializeReference] private AttackData m_attackData;

    public string AttackPointId => m_attackPointId;
    public AttackData AttackData => m_attackData;
}

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
#if UNITY_EDITOR
    [SerializeField] private MonoScript m_enemyScript;
#endif

    // 実行時のEnemy型チェック用
    [SerializeField, HideInInspector]
    private string m_enemyTypeName;

    [SerializeField] private List<EnemyAttackSetting> m_attackSettings = new();

    public List<EnemyAttackSetting> AttackSettings => m_attackSettings;

    public bool MatchesEnemyType(System.Type enemyType)
    {
        return enemyType != null &&
               m_enemyTypeName == enemyType.FullName;
    }



    public AttackData GetAttackData(string id)
    {
        foreach (var setting in m_attackSettings)
        {
            if (setting.AttackPointId == id)
            {
                return setting.AttackData;
            }
        }

        Debug.LogError($"AttackDataが見つかりません。ID: {id}");
        return null;
    }
}

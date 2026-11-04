using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeReference]
    private List<AttackData> m_attackList = new()
    {
        new MeleeAttackData()
    };
}

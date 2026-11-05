using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "EnemyData", menuName = "Scriptable Objects/EnemyData")]
public class EnemyData : ScriptableObject
{
    [SerializeField] private int m_str = 1;

    [SerializeField] private float m_speed = 5f;



    [SerializeReference]
    private List<AttackData> m_attackList = new();


}

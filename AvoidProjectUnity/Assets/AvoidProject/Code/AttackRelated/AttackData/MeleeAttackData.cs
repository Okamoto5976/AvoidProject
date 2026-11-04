using UnityEngine;
using System.Collections.Generic;

[CreateAssetMenu(fileName = "SwordAttackData", menuName = "Scriptable Objects/Data/MeleeAttackData")]
public class MeleeAttackData : AttackData
{
    [SerializeField] private float m_radius;
    [SerializeField] private float m_size;

    //Actionのデータの並び これだとSOが共通されてしまってるので　SO外にもつ必要がある
    public List<ActionData> m_actionDatas = new();
}

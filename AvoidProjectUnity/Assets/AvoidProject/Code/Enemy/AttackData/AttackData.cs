using UnityEngine;

[CreateAssetMenu(fileName = "AttackData", menuName = "Scriptable Objects/Data/AttackData")]
public abstract class AttackData : ScriptableObject
{
    public abstract IAttack CreateAttack();
}

using UnityEngine;

[CreateAssetMenu(fileName = "ActionData", menuName = "Scriptable Objects/ActionData")]
public abstract class ActionData : ScriptableObject
{
    public abstract Action CreateAction();
}

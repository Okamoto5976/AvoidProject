using UnityEngine;

[CreateAssetMenu(fileName = "MeleeActionData", menuName = "Scriptable Objects/MeleeActionData")]
public class MeleeActionData : ActionData
{
    public float a;//発動タイミング

    public override Action CreateAction()
    {
        return new MeleeAction(this);
    }
}

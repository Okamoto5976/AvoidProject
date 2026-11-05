using UnityEngine;

public interface IAttack
{
    public event System.Action<Vector2, float> Movement;
    void Execute();

    void Update();
}

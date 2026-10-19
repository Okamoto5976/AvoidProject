using UnityEngine;

public enum ShapeType
{
    Box,
    Circle,
    //Capsule
}

[System.Serializable]
public class Attack
{
    public ShapeType ShapeType;
    public Vector2 Size;
    public Vector2 Position;
    public float StartupTime;
    public float ActiveTime;
}

public class AttackManager : MonoBehaviour
{
    [SerializeField] private ColliderManager m_ColliderManager;
}

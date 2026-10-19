using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [SerializeField] private int m_hp = 3;
    [SerializeField] private int m_dhp = 0;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private float m_avoidSpeedRate = 1.5f;
    [SerializeField] private float m_hitRadius = 1f;
    [SerializeField] private int m_justAvoidFlame = 5;

    public int HP => m_hp;
    public int Dhp => m_dhp;
    public float Speed => m_speed;
    public float AvoidSpeedRate => m_avoidSpeedRate;
    public float HitRadius => m_hitRadius;
    public int JustAvoidFlame => m_justAvoidFlame;
}

using UnityEngine;

[CreateAssetMenu(fileName = "PlayerData", menuName = "Scriptable Objects/Data/PlayerData")]
public class PlayerData : ScriptableObject
{
    [Header("基本設定")]
    [SerializeField] private int m_hp = 3;
    [SerializeField] private int m_dhp = 0;
    [SerializeField] private float m_speed = 5f;
    [SerializeField] private float m_sprintSpeed = 10f;
    [SerializeField] private float m_attackStateDuration = 0.3f;

    [Header("回避設定")]
    [SerializeField] private float m_avoidSpeedRate = 1.5f;
    [SerializeField] private int m_avoidStartFrame = 5;
    [SerializeField] private float m_avoidDuration = 0.2f;
    [SerializeField] private int m_justAvoidFrame = 5;

    [Header("インタラクト設定")]
    [SerializeField] private float m_hitRadius = 1f;
    [SerializeField] private float m_interactRange = 2f;
    [SerializeField] private float m_interactHoldTime = 1.5f;

    [Header("回復設定")]
    [SerializeField] private float m_recoverMoveSpeed = 2f;
    [SerializeField] private float m_recoverDuration = 2f;
    [SerializeField] private int m_recoverValue = 100;

    public int HP => m_hp;
    public int Dhp => m_dhp;
    public float Speed => m_speed;
    public float SprintSpeed => m_sprintSpeed;
    public float AttackStateDuration => m_attackStateDuration;

    public float AvoidSpeedRate => m_avoidSpeedRate;
    public int AvoidStartFrame => m_avoidStartFrame;
    public float AvoidDuration => m_avoidDuration;
    public int JustAvoidFrame => m_justAvoidFrame;

    public float HitRadius => m_hitRadius;
    public float InteractRange => m_interactRange;
    public float InteractHoldTime => m_interactHoldTime;

    public float RecoverMoveSpeed => m_recoverMoveSpeed;
    public float RecoverDuration => m_recoverDuration;
    public int RecoverValue => m_recoverValue;
}
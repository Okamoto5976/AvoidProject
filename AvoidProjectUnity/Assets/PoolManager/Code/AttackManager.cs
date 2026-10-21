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

[System.Serializable]
public class TimedAction
{
    public Attack Attack;
    public float Timer;
    public GameObject Object;
}

public class AttackManager : MonoBehaviour
{
    [SerializeField] private ColliderManager m_ColliderManager;
    [SerializeField] private AttackViewManager m_AttackViewManager;

    /// <summary>
    /// 攻撃する関数
    /// </summary>
    /// <param name="attack">攻撃のデータ</param>
    public void Attack(Attack attack)
    {
        //Debug.Log("攻撃スタート");
        if (attack.StartupTime > 0)
        {
            m_AttackViewManager.AttackView(attack);
        }
        else
        {
            m_ColliderManager.ActiveCollider(attack);
        }
    }

    /// <summary>
    /// 攻撃予告を表示する時間を終えたら呼ばれる関数
    /// </summary>
    /// <param name="attack">攻撃のデータ</param>
    public void EndAttackView(Attack attack)
    {
        m_ColliderManager.ActiveCollider(attack);
    }
}

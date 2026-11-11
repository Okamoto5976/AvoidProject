using UnityEngine;

public enum ShapeType
{
    Box,
    Circle
}

[System.Serializable]
public class Attack
{
    public ShapeType ShapeType;
    public Vector2 Size;
    public Vector2 Position;
    public float StartupTime;
    public float ActiveTime;

    public int Damage;

    public Attack Clone()
    {
        Attack attack = new Attack();
        attack.ShapeType = ShapeType;
        attack.Size = Size;
        attack.Position = Position;
        attack.StartupTime = StartupTime;
        attack.ActiveTime = ActiveTime;
        attack.Damage = Damage;
        return attack;
    }
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
    private ColliderManager m_ColliderManager;
    private AttackViewManager m_AttackViewManager;

    public void SetColliderManager(ColliderManager component)
    {
        m_ColliderManager = component;
    }

    public void SetAttackViewManager(AttackViewManager component)
    {
        m_AttackViewManager = component;
    }

    /// <summary>
    /// 攻撃する関数
    /// </summary>
    /// <param name="attack">攻撃のデータ</param>
    public void Attack(Attack attack)
    {
        //Debug.Log("攻撃スタート");
        Attack cloneAttack = attack.Clone();
        if (cloneAttack.StartupTime > 0)
        {
            m_AttackViewManager.AttackView(cloneAttack);
        }
        else
        {
            m_ColliderManager.ActiveCollider(cloneAttack);
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

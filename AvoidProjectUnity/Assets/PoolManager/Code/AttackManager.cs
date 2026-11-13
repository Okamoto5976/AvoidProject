using UnityEngine;

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
        if (CheckError(attack.ColliderProperty))
        {
            Debug.LogWarning($"値の設定が間違っており、攻撃できませんでした。(ShapeType == {attack.ColliderProperty.ShapeType})");
        }


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

    private bool CheckError(ColliderProperty property)
    {
        switch (property.ShapeType)
        {
            case ShapeType.Circle:
                if (property.Radius == 0)    return true;
                break;
            case ShapeType.Box:
                if (property.Width == 0)     return true;
                if (property.Height == 0)    return true;
                break;
            case ShapeType.Line:
                if (property.Thickness == 0) return true;
                if (property.Length == 0)    return true;
                break;
            case ShapeType.Sector:
                if (property.Radius == 0)    return true;
                if (property.Angle == 0)     return true;
                break;
        }
        return false;
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

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
        //特殊処理
        //if (attack.ColliderProperty.ShapeType == ShapeType.Line)
        //{
        //    attack.Position.y += attack.ColliderProperty.Length / 2;
        //}


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

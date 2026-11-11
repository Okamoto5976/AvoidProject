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
    [SerializeField] private ColliderManager m_ColliderManager;
    [SerializeField] private AttackViewManager m_AttackViewManager;

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

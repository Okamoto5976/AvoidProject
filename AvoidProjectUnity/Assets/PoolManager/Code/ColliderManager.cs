using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] private PoolManager m_BoxColliderPool;
    [SerializeField] private PoolManager m_CiecleColliderPool;
    [SerializeField] private Rigidbody2D m_Player;

    //確認用
    [SerializeField] private List<TimedAction> m_TimedActions;

    private void Update()
    {
        for (int i = m_TimedActions.Count - 1; i >= 0; i--)
        {
            m_TimedActions[i].Timer -= Time.deltaTime;
            if (m_TimedActions[i].Timer < 0)
            {
                //Debug.Log("コライダー無効化");
                ReturnCollider(m_TimedActions[i]);
                m_TimedActions[i] = m_TimedActions[m_TimedActions.Count - 1];
                m_TimedActions.RemoveAt(m_TimedActions.Count - 1);
            }
        }
    }

    private void ReturnCollider(TimedAction action)
    {
        switch (action.Attack.ShapeType)
        {
            case ShapeType.Box:
                m_BoxColliderPool.ReturnToPool(action.Object);
                break;
            case ShapeType.Circle:
                m_CiecleColliderPool.ReturnToPool(action.Object);
                break;
        }
    }

    /// <summary>
    /// 判定用のColliderを生成する関数
    /// </summary>
    /// <param name="attack">攻撃のデータ</param>
    public void ActiveCollider(Attack attack)
    {
        //Debug.Log("コライダー有効化");
        GameObject obj = null;
        switch (attack.ShapeType)
        {
            case ShapeType.Box:
                obj = m_BoxColliderPool.GenerateFromPool();
                //BoxCollider2D boxCollider = obj.GetComponent<BoxCollider2D>();
                //boxCollider.size = attack.Size;
                break;
            case ShapeType.Circle:
                obj = m_CiecleColliderPool.GenerateFromPool();
                //CircleCollider2D circleCollider2D = obj.GetComponent<CircleCollider2D>();
                //circleCollider2D.radius = attack.Size.x / 2;
                break;
        }
        if (obj == null) return;

        obj.transform.position = attack.Position;
        obj.transform.localScale = attack.Size;
        TimedAction action = new();
        action.Attack = attack;
        action.Timer = attack.ActiveTime;
        action.Object = obj;
        m_TimedActions.Add(action);
        m_Player.WakeUp();
    }
}

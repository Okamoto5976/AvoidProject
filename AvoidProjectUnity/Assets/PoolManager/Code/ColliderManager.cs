using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] private ColliderMediationPool m_MediationPool;
    [SerializeField] private Rigidbody2D m_Player;

    private List<TimedAction> m_TimedActions = new();

    private void Update()
    {
        for (int i = m_TimedActions.Count - 1; i >= 0; i--)
        {
            m_TimedActions[i].Timer -= Time.deltaTime;
            if (m_TimedActions[i].Timer < 0)
            {
                //Debug.Log("コライダー無効化");
                ReturnPool returnPool = m_TimedActions[i].Object.GetComponent<ReturnPool>();
                returnPool.Return();
                m_TimedActions[i] = m_TimedActions[m_TimedActions.Count - 1];
                m_TimedActions.RemoveAt(m_TimedActions.Count - 1);
            }
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
        PoolManager pool = m_MediationPool.GetColliderPool(attack.ShapeType);
        obj = pool.GenerateFromPool();
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

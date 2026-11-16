using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColliderManager : MonoBehaviour
{
    [SerializeField] private PoolManager m_CustomColliderPool;

    [SerializeField] private Rigidbody2D m_Player;
    private ColliderMediationPool m_MediationPool;
    //[SerializeField] private Rigidbody2D m_Player;

    private List<TimedAction> m_TimedActions = new();

    private List<Collider2D> m_DrawColliders = new();

    private Color m_ColliderColor = Color.green;

    //一瞬だけ表示する場合用
    private float m_GizmoViewDelay = 2f;

    public void SetColliderMediationPool(ColliderMediationPool component)
    {
        m_MediationPool = component;
    }

    private void Update()
    {
        for (int i = m_TimedActions.Count - 1; i >= 0; i--)
        {
            m_TimedActions[i].Timer -= Time.deltaTime;
            if (m_TimedActions[i].Timer < 0)
            {
                RemoveGizmo(m_TimedActions[i].Object.GetComponent<Collider2D>());

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
        //PoolManager pool = m_MediationPool.GetColliderPool(attack.ShapeType);
        //obj = pool.GenerateFromPool();
        obj = m_CustomColliderPool.GenerateFromPool();
        if (obj == null) return;
        

        Collider2D col = obj.GetComponent<Collider2D>();
        col.excludeLayers = 0;

        obj.transform.position = attack.Position;
        //obj.transform.localScale = attack.Size;

        var component = obj.GetComponent<ColliderAttack>();

        //component.Initialize(attack.Damage, attack.Team);

        AddGizmo(col);

        TimedAction action = new();
        action.Attack = attack;
        action.Object = obj;
        if (attack.ActiveTime > 0)
        {
            action.Timer = attack.ActiveTime;
        }
        else
        {
            action.Timer = m_GizmoViewDelay;
            StartCoroutine(OnceFlameCollider(col));
        }
        m_TimedActions.Add(action);
        m_Player.WakeUp();

        ColliderController colliderController = obj.GetComponent<ColliderController>();
        colliderController.SetColliderProperty(attack.ColliderProperty, attack.Position, attack.Parent);
        //m_Player.WakeUp();
    }

    private IEnumerator OnceFlameCollider(Collider2D col)
    {
        //Debug.Log("OnceFlameCollider");
        yield return new WaitForFixedUpdate(); 
        yield return new WaitForFixedUpdate();
        col.excludeLayers = ~0;
    }

    private void AddGizmo(Collider2D col)
    {
        m_DrawColliders.Add(col);
    }

    private void RemoveGizmo(Collider2D col)
    {
        m_DrawColliders.Remove(col);
    }

    private void OnDrawGizmos()
    {
        foreach (Collider2D col in m_DrawColliders)
        {
            if (col == null || !col.enabled) continue;

            Gizmos.color = m_ColliderColor;
            Gizmos.matrix = col.transform.localToWorldMatrix;

            switch (col)
            {
                case BoxCollider2D box:
                    //Gizmos.DrawCube(box.offset, box.size);
                    Gizmos.DrawWireCube(box.offset, box.size);
                    break;
                case CircleCollider2D circle:
                    //Gizmos.DrawSphere(circle.offset, circle.radius);
                    DrawWireCircle(circle.offset, circle.radius);
                    break;
            }
        }
        Gizmos.matrix = Matrix4x4.identity;
    }

    private void DrawWireCircle(Vector2 center, float radius, int segments = 32)
    {
        Vector3 prev = center + new Vector2(radius, 0f);
        for (int i = 1; i <= segments; i++)
        {
            float a = 2f * Mathf.PI * i / segments;
            Vector3 next = center + new Vector2(Mathf.Cos(a), Mathf.Sin(a)) * radius;
            Gizmos.DrawLine(prev, next);
            prev = next;
        }
    }
}
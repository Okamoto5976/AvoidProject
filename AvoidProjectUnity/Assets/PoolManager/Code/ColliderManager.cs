using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[System.Serializable]
public class TimedAction
{
    public ShapeType ShapeType;
    public float Timer;
    public GameObject Object;
}

public class ColliderManager : MonoBehaviour
{
    [SerializeField] private PoolManager m_BoxColliderPool;
    [SerializeField] private PoolManager m_CiecleColliderPool;

    //Šm”F—p
    [SerializeField] private List<TimedAction> m_TimedActions;
    
    //[SerializeField] private Attack test;

    private void Update()
    {
        for (int i = m_TimedActions.Count - 1; i >= 0; i--)
        {
            m_TimedActions[i].Timer -= Time.deltaTime;
            if (m_TimedActions[i].Timer < 0)
            {
                ReturnCollider(m_TimedActions[i]);
                m_TimedActions[i] = m_TimedActions[m_TimedActions.Count - 1];
                m_TimedActions.RemoveAt(m_TimedActions.Count - 1);
            }
        }

        //test
        //if (Mouse.current.leftButton.wasPressedThisFrame)
        //{
        //    Attack(test);
        //}
    }

    public void ActiveCollider(Attack attack)
    {
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
        action.ShapeType = attack.ShapeType;
        action.Timer = attack.ActiveTime;
        action.Object = obj;
        m_TimedActions.Add(action);
    }

    private void ReturnCollider(TimedAction action)
    {
        switch (action.ShapeType)
        {
            case ShapeType.Box:
                m_BoxColliderPool.ReturnToPool(action.Object);
                break;
            case ShapeType.Circle:
                m_CiecleColliderPool.ReturnToPool(action.Object);
                break;
        }
    }
}

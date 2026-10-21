using System.Collections.Generic;
using UnityEngine;

public class AttackViewManager : MonoBehaviour
{
    [SerializeField] private AttackManager m_AttackManager;
    [SerializeField] private PoolManager m_AttackViewPool;
    [SerializeField] private Sprite m_Box;
    [SerializeField] private Sprite m_Circle;

    //ämîFóp
    [SerializeField] private List<TimedAction> m_TimedActions;

    private void Update()
    {
        for (int i = m_TimedActions.Count - 1; i >= 0; i--)
        {
            m_TimedActions[i].Timer -= Time.deltaTime;
            if (m_TimedActions[i].Timer < 0)
            {
                //Debug.Log("çUåÇó\çêîÒï\é¶");
                m_AttackViewPool.ReturnToPool(m_TimedActions[i].Object);
                m_AttackManager.EndAttackView(m_TimedActions[i].Attack);
                m_TimedActions[i] = m_TimedActions[m_TimedActions.Count - 1];
                m_TimedActions.RemoveAt(m_TimedActions.Count - 1);
            }
        }
    }

    /// <summary>
    /// çUåÇó\çêÇï\é¶Ç∑ÇÈä÷êî
    /// </summary>
    /// <param name="attack">çUåÇÇÃÉfÅ[É^</param>
    public void AttackView(Attack attack)
    {
        //Debug.Log("çUåÇó\çêï\é¶");
        GameObject obj = m_AttackViewPool.GenerateFromPool();
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();
        switch (attack.ShapeType)
        {
            case ShapeType.Box:
                spriteRenderer.sprite = m_Box;
                break;
            case ShapeType.Circle:
                spriteRenderer.sprite = m_Circle;
                break;
        }
        obj.transform.position = attack.Position;
        obj.transform.localScale = attack.Size;
        TimedAction action = new();
        action.Attack = attack;
        action.Timer = attack.StartupTime;
        action.Object = obj;
        m_TimedActions.Add(action);
    }
}

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class AttackViewManager : MonoBehaviour
{
    private AttackManager m_AttackManager;
    private PoolManager m_AttackViewPool;
    [SerializeField] private Sprite m_Box;
    [SerializeField] private Sprite m_Circle;

    private List<TimedAction> m_TimedActions = new();

    public void SetAttackManager(AttackManager component)
    {
        m_AttackManager = component;
    }

    public void SetAttackViewPool(PoolManager component)
    {
        m_AttackViewPool = component;
    }

    private void Update()
    {
        for (int i = m_TimedActions.Count - 1; i >= 0; i--)
        {
            m_TimedActions[i].Timer -= Time.deltaTime;
            if (m_TimedActions[i].Timer < 0)
            {
                //Debug.Log("çUåÇó\çêîÒï\é¶");
                ReturnPool returnPool = m_TimedActions[i].Object.GetComponent<ReturnPool>();
                returnPool.Return();
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
        Transform transform = obj.transform;
        SpriteRenderer spriteRenderer = obj.GetComponent<SpriteRenderer>();

        transform.localPosition = attack.Position;
        transform.parent = attack.Parent;

        switch (attack.ColliderProperty.ShapeType)
        {
            case ShapeType.Box:
                ShapingBox(attack.ColliderProperty, spriteRenderer, transform);
                break;
            case ShapeType.Circle:
                ShapingCircle(attack.ColliderProperty, spriteRenderer, transform);
                break;
            case ShapeType.Line:
                ShapingLine(attack.ColliderProperty, spriteRenderer, transform);
                break;
        }

        TimedAction action = new();
        action.Attack = attack;
        action.Timer = attack.StartupTime;
        action.Object = obj;
        m_TimedActions.Add(action);
    }

    private void ShapingBox(ColliderProperty cp, SpriteRenderer sr, Transform tf)
    {
        sr.sprite = m_Box;
        float width = cp.Width;
        float height = cp.Height;
        float rotate = cp.Rotate;
        tf.localScale = new Vector2(width, height);
        tf.rotation = new Quaternion(0, 0, rotate, 0);
    }

    private void ShapingCircle(ColliderProperty cp, SpriteRenderer sr, Transform tf)
    {
        sr.sprite = m_Circle;
        float diameter = cp.Radius * 2;
        tf.localScale = new Vector2(diameter, diameter);
    }

    private void ShapingLine(ColliderProperty cp, SpriteRenderer sr, Transform tf)
    {
        sr.sprite = m_Box;
        float thickness = cp.Thickness;
        float length = cp.Length;
        float direction = cp.Direction;
        tf.localScale = new Vector2(thickness, length);
        float rad = direction * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));
        Vector2 center = dir * (length * 0.5f);
        tf.localPosition += (Vector3)center;
        tf.rotation = Quaternion.Euler(0, 0, direction);
    }
}

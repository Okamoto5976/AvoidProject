using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public struct ColliderProperty
{
    public ShapeType ShapeType;
    //Circle and Sector
    public float Radius;
    //Box
    public float Width;
    public float Height;
    //Line
    public float Length;
    public float Thickness;
    //Sector
    public float Angle;
}

public class ColliderController : MonoBehaviour
{
    private CustomCollider2D m_Collider;

    private ColliderProperty m_ColliderProperty;
    private Vector2 m_Position;

    private PhysicsShapeGroup2D m_ShapeGroup = new PhysicsShapeGroup2D();

    public void SetColliderProperty(ColliderProperty setProperty, Vector2 setPos)
    {
        if (m_Collider == null)
        {
            m_Collider = GetComponent<CustomCollider2D>();
        }
        m_ColliderProperty = setProperty;
        m_Position = setPos;
        EditCollider();
    }

    private int m_MaxArcSeg = 6;

    private void EditCollider()
    {
        m_ShapeGroup.Clear();
        switch (m_ColliderProperty.ShapeType)
        {
            case ShapeType.Circle:
                m_ShapeGroup.AddCircle(m_Position, m_ColliderProperty.Radius);
                break;
            case ShapeType.Box:
                Vector2 boxSize = new Vector2(m_ColliderProperty.Width, m_ColliderProperty.Height);
                m_ShapeGroup.AddBox(m_Position, boxSize);
                break;
            case ShapeType.Line:
                Vector2 lineSize = new Vector2(m_ColliderProperty.Thickness, m_ColliderProperty.Length);
                m_Position.y += m_ColliderProperty.Length / 2;
                m_ShapeGroup.AddBox(m_Position, lineSize);
                break;
            case ShapeType.Sector:
                List<Vector2> vertices = new(8);
                int segments = 12;
                if (m_ColliderProperty.Angle > 180) segments *= 2;
                float start = 90 - m_ColliderProperty.Angle * 0.5f;
                float step = m_ColliderProperty.Angle / segments;

                for (int i = 0; i < segments; i += m_MaxArcSeg)
                {
                    int count = Mathf.Min(m_MaxArcSeg, segments - i);
                    vertices.Clear();
                    vertices.Add(m_Position);

                    for (int j = 0; j <= count; j++)
                    {
                        float radian = (start + (i + j) * step) * Mathf.Deg2Rad;
                        vertices.Add(new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * m_ColliderProperty.Radius);
                    }
                    m_ShapeGroup.AddPolygon(vertices);
                }
                m_Collider.SetCustomShapes(m_ShapeGroup);
                break;
        }
        m_Collider.SetCustomShapes(m_ShapeGroup);
    }
}
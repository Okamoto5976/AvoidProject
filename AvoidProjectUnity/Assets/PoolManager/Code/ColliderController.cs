using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;

public class ColliderController : MonoBehaviour
{
    private Vector2 m_Position;

    private CustomCollider2D m_Collider;

    private ColliderProperty m_Property;

    private PhysicsShapeGroup2D m_ShapeGroup = new();

    public void SetColliderProperty(ColliderProperty setProperty, Vector2 setPosition, Transform setParent = null)
    {
        if (m_Collider == null)
        {
            m_Collider = GetComponent<CustomCollider2D>();
        }
        if (setParent != null)
        {
            m_Collider.transform.parent = setParent;
        }
        m_Property = setProperty;
        m_Position = setPosition;
        m_Collider.offset = m_Position;
        EditCollider();
    }

    private int m_MaxArcSeg = 6;

    private void EditCollider()
    {
        m_ShapeGroup.Clear();
        switch (m_Property.ShapeType)
        {
            case ShapeType.Circle:
                m_ShapeGroup.AddCircle(Vector2.zero, m_Property.Radius);
                break;
            case ShapeType.Box:
                Vector2 boxSize = new Vector2(m_Property.Width, m_Property.Height);
                m_ShapeGroup.AddBox(Vector2.zero, boxSize, m_Property.Rotate);
                break;
            case ShapeType.Line:
                EditLine(m_Property.Thickness, m_Property.Length, m_Property.Direction);
                break;
            case ShapeType.Sector:
                EditSector(m_Property.Radius, m_Property.Angle, m_Property.Direction);
                break;
        }
        m_Collider.SetCustomShapes(m_ShapeGroup);
    }

    private void EditLine(float thickness, float length, float direction)
    {
        float rad = direction * Mathf.Deg2Rad;
        Vector2 dir = new Vector2(-Mathf.Sin(rad), Mathf.Cos(rad));

        Vector2 center = dir * (length * 0.5f);
        Vector2 size = new Vector2(thickness, length);

        m_ShapeGroup.AddBox(center, size, direction);
    }

    private void EditSector(float radius, float angle, float euler)
    {
        List<Vector2> vertices = new(8);
        int segments = 12;
        if (angle > 180) segments *= 2;
        float start = 90 + euler - angle * 0.5f;
        float step = angle / segments;

        for (int i = 0; i < segments; i += m_MaxArcSeg)
        {
            int count = Mathf.Min(m_MaxArcSeg, segments - i);
            vertices.Clear();
            vertices.Add(Vector2.zero);

            for (int j = 0; j <= count; j++)
            {
                float radian = (start + (i + j) * step) * Mathf.Deg2Rad;
                vertices.Add(new Vector2(Mathf.Cos(radian), Mathf.Sin(radian)) * radius);
            }
            m_ShapeGroup.AddPolygon(vertices);
        }
        m_Collider.SetCustomShapes(m_ShapeGroup);
    }
}
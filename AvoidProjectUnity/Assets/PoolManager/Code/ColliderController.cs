using System.Collections.Generic;
using UnityEngine;

public class ColliderController : MonoBehaviour
{
    private Vector2 m_Position;
    private Vector3 m_RotationEuler;

    private CustomCollider2D m_Collider;

    private ColliderProperty m_ColliderProperty;

    private PhysicsShapeGroup2D m_ShapeGroup = new();

    public void SetColliderProperty(ColliderProperty setProperty, Vector2 setPosition, Vector3 setRotationEuler)
    {
        if (m_Collider == null)
        {
            m_Collider = GetComponent<CustomCollider2D>();
        }
        m_ColliderProperty = setProperty;
        m_Position = setPosition;
        m_RotationEuler = setRotationEuler;
        m_Collider.offset = m_Position;
        EditCollider();
    }

    private int m_MaxArcSeg = 6;

    private void EditCollider()
    {
        m_ShapeGroup.Clear();
        switch (m_ColliderProperty.ShapeType)
        {
            case ShapeType.Circle:
                m_ShapeGroup.AddCircle(Vector2.zero, m_ColliderProperty.Radius);
                break;
            case ShapeType.Box:
                Vector2 boxSize = new Vector2(m_ColliderProperty.Width, m_ColliderProperty.Height);
                m_ShapeGroup.AddBox(Vector2.zero, boxSize, m_RotationEuler.z);
                break;
            case ShapeType.Line:
                Vector2 lineSize = new Vector2(m_ColliderProperty.Thickness, m_ColliderProperty.Length);
                //Vector2 delayPos = new Vector2(0, lineSize.y / 2);
                m_ShapeGroup.AddBox(Vector2.zero, lineSize, m_RotationEuler.z);
                break;
            case ShapeType.Sector:
                EditSector(m_ColliderProperty.Radius, m_ColliderProperty.Angle, m_RotationEuler.z);
                break;
            case ShapeType.Other:
                m_ShapeGroup.AddPolygon(m_ColliderProperty.Vertices);
                break;
        }
        m_Collider.SetCustomShapes(m_ShapeGroup);
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
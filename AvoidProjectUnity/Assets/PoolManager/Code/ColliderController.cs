using System;
using UnityEngine;

[Serializable]
public class ColliderProperty
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

    private PhysicsShapeGroup2D m_ShapeGroup = new PhysicsShapeGroup2D();

    public void SetColliderProperty(ColliderProperty setProperty)
    {
        m_ColliderProperty = setProperty;
        EditCollider();
    }

    private void OnEnable()
    {
        if (m_Collider == null)
        {
            m_Collider = GetComponent<CustomCollider2D>();
        }
    }

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
                m_ShapeGroup.AddBox(Vector2.zero, boxSize);
                break;
            case ShapeType.Line:
                Vector2 lineSize = new Vector2(m_ColliderProperty.Length, m_ColliderProperty.Thickness);
                m_ShapeGroup.AddBox(Vector2.zero, lineSize);
                break;
            case ShapeType.Sector:

                break;
        }
    }
}
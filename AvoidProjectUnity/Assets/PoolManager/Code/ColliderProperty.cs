using System;
using UnityEditor;
using UnityEngine;

public enum ShapeType
{
    Box,
    Circle,
    Line,
    Sector,
    //Donut
}

[Serializable]
public struct ColliderProperty
{
    public ShapeType ShapeType;
    //Circle and Sector
    public float Radius;
    //Line and Sector
    public float Direction;
    //Box
    public float Width;
    public float Height;
    public float Rotate;
    //Line
    public float Thickness;
    public float Length;
    //Sector
    public float Angle;
    //Donut
    //public float OutRadius;
    //public float InRadius;
}

#if UNITY_EDITOR

[CustomPropertyDrawer(typeof(ColliderProperty))]
public class ColliderPropertyEditor : PropertyDrawer
{
    // Enum の値ごとに表示するフィールド名
    private static string[] GetVisibleFields(ShapeType type)
    {
        switch (type)
        {
            case ShapeType.Box      : return new[] { "Width", "Height", "Rotate" };
            case ShapeType.Circle   : return new[] { "Radius" };
            case ShapeType.Line     : return new[] { "Thickness", "Length", "Direction" };
            case ShapeType.Sector   : return new[] { "Radius", "Angle", "Direction" };
            //case ShapeType.Donut    : return new[] { "OutRadius", "InRadius" };
            default: return new string[0];
        }
    }

    public override float GetPropertyHeight(SerializedProperty property, GUIContent label)
    {
        //行と余白の高さ
        float line = EditorGUIUtility.singleLineHeight;
        float space = EditorGUIUtility.standardVerticalSpacing;

        //プロパティが開かれているか
        if (!property.isExpanded) return line;

        var enumProperty = property.FindPropertyRelative("ShapeType");
        float height = line + space;
        height += EditorGUI.GetPropertyHeight(enumProperty) + space;

        foreach (var name in GetVisibleFields((ShapeType)enumProperty.enumValueIndex))
        {
            var _property = property.FindPropertyRelative(name);
            height += EditorGUI.GetPropertyHeight(_property, true) + space;
        }
        return height;
    }

    public override void OnGUI(Rect position, SerializedProperty property, GUIContent label)
    {
        EditorGUI.BeginProperty(position, label, property);

        float space = EditorGUIUtility.standardVerticalSpacing;
        Rect rect = new(position.x, position.y, position.width, EditorGUIUtility.singleLineHeight);

        property.isExpanded = EditorGUI.Foldout(rect, property.isExpanded, label, true);
        rect.y += rect.height + space;

        if (property.isExpanded)
        {
            EditorGUI.indentLevel++;

            var enumProperty = property.FindPropertyRelative("ShapeType");
            rect.height = EditorGUI.GetPropertyHeight(enumProperty);
            EditorGUI.PropertyField(rect, enumProperty);
            rect.y += rect.height + space;

            foreach (var name in GetVisibleFields((ShapeType)enumProperty.enumValueIndex))
            {
                var _property = property.FindPropertyRelative(name);
                rect.height = EditorGUI.GetPropertyHeight(_property, true);
                EditorGUI.PropertyField(rect, _property, true);
                rect.y += rect.height + space;
            }

            EditorGUI.indentLevel--;
        }
        EditorGUI.EndProperty();
    }
}

#endif
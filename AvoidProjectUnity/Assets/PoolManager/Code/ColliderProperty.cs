using UnityEngine;
using System;
using System.Collections.Generic;

public enum ShapeType
{
    Box,
    Circle,
    Line,
    Sector,
    Other
}

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
    //Other
    public List<Vector2> Vertices;
}
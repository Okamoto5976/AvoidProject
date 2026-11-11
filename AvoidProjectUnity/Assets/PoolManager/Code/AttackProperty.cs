using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class Attack
{
    //public List<ColliderProperty> ColliderProperties = new();
    public ColliderProperty ColliderProperty = new();
    public Vector2 Position;
    public Transform Parent;
    public float StartupTime;
    public float ActiveTime;

    public Attack Clone() => (Attack)this.MemberwiseClone();
}
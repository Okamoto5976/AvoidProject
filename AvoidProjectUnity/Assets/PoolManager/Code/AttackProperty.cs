using UnityEngine;
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;

[Serializable]
public class Attack
{
    public Team Team;

    public int Damage;

    //public List<ColliderProperty> ColliderProperties = new();
    public ColliderProperty ColliderProperty = new();
    public List<Vector2> Positions;
    public Transform Parent;
    public float StartupTime;
    public float ActiveTime;

    //public Attack Clone() => (Attack)this.MemberwiseClone();

    public Attack Clone()
    {
        Attack attack = (Attack)this.MemberwiseClone();
        attack.Positions = new List<Vector2>(attack.Positions);
        return attack;
    }
}
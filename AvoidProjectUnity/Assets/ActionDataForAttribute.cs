using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Class, AllowMultiple = true)]
public class ActionDataForAttribute : Attribute
{
    public Type AttackDataType { get; }

    public ActionDataForAttribute(Type attackDataType)
    {
        AttackDataType = attackDataType;
    }
}

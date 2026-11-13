using System;
using UnityEngine;

[AttributeUsage(AttributeTargets.Method, Inherited = true)]
public class EnemyAttackPointAttribute : Attribute
{
    public string Id { get; }
    public string DisplayName { get; }
    public string Description { get; }

    public EnemyAttackPointAttribute(
        string id,
        string displayName,
        string description)
    {
        Id = id;
        DisplayName = displayName;
        Description = description;
    }
}

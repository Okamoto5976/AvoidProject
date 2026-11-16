using UnityEngine;

public enum DebuffCategory
{
    Action,
    Enemy,
    Life,
    Item
}

public class Debuff
{
    public int Id;
    public string Name;
    public int Reward;
    public DebuffCategory Category;
}

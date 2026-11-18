using System;

[Serializable]
public enum DebuffCategory
{
    Action,
    Enemy,
    Life,
    Item
}

[Serializable]
public class Debuff
{
    public int Id;
    public string Name;
    public int Reward;
    public DebuffCategory Category;
}

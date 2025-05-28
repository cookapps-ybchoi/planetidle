using UnityEngine;

public enum InGameSkillId
{
    MultiShot = 101,
    AttackSpeed = 102,
    AttackPower = 103,
    AttackRange = 104,
    Hp = 105,
}

public enum InGameSkillType
{
    Passive,
    Active,
}


public class InGameSkillMetaData
{
    public string Name { get; private set; }
    public string Description { get; private set; }
    public InGameSkillId Id { get; private set; }
    public InGameSkillType Type { get; private set; }
    public int MaxLevel { get; private set; }
    public double FirstValue { get; private set; }
    public double SecondValue { get; private set; }

    public InGameSkillMetaData(string name, string description, InGameSkillId id, InGameSkillType type, int maxLevel, double firstValue, double secondValue)
    {
        Name = name;
        Description = description;
        Id = id;
        Type = type;
        MaxLevel = maxLevel;
        FirstValue = firstValue;
        SecondValue = secondValue;
    }
}

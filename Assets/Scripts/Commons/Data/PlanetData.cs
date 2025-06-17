using System;
using UnityEngine;

public enum PlanetStatType
{
    PlanetLevel,
    AttackPower,
    AttackSpeed,
    Hp,
    MaxHp,
    HpRecovery,
    AttackCooltime,
    Range,
    ShotCount,

}

[Serializable]
public class PlanetData
{
    public PlanetEntity Entity { get; private set; }

    public int PlanetId { get => Entity.planetId; }
    public int PlanetLevel { get => Entity.planetLevel; }

    public double MaxHp { get; set; }
    public double Hp { get; set; }
    public double HpRecovery { get; set; }
    public double AttackRange { get; set; }
    public double AttackPower { get; set; }
    public double AttackSpeed { get; set; }
    public double AttackCooltime { get; set; }
    public double AttackCount { get; set; }

    public PlanetData(PlanetEntity entity)
    {
        Entity = entity;

        MaxHp = Entity.hp;
        Hp = Entity.hp;
        HpRecovery = Entity.hpRecovery;
        AttackRange = Entity.attackRange;
        AttackPower = Entity.attackPower;
        AttackSpeed = Entity.attackSpeed;
        AttackCooltime = Entity.attackCooltime;
        AttackCount = Entity.attackCount;
    }

    public double GetStateValue(PlanetStatType statType)
    {
        return statType switch
        {
            PlanetStatType.Hp => Hp,
            PlanetStatType.MaxHp => MaxHp,
            PlanetStatType.HpRecovery => HpRecovery,
            PlanetStatType.Range => AttackRange,
            PlanetStatType.AttackPower => AttackPower,
            PlanetStatType.AttackSpeed => AttackSpeed,
            PlanetStatType.AttackCooltime => AttackCooltime,
            PlanetStatType.ShotCount => AttackCount,
            _ => 0f
        };
    }

    public void SetStateValue(PlanetStatType statType, double value)
    {
        switch (statType)
        {
            case PlanetStatType.Hp:
                Hp = value;
                break;
            case PlanetStatType.MaxHp:
                //MaxHP 증가량 만큼 Hp 값 증가
                Hp += value - MaxHp;
                InGameEventHandler.InvokePlanetStateValueChanged(PlanetStatType.Hp, Hp);
                MaxHp = value;
                break;
            case PlanetStatType.HpRecovery:
                HpRecovery = value;
                break;
            case PlanetStatType.Range:
                AttackRange = value;
                break;
            case PlanetStatType.AttackPower:
                AttackPower = value;
                break;
            case PlanetStatType.AttackSpeed:
                AttackSpeed = value;
                break;
            case PlanetStatType.AttackCooltime:
                AttackCooltime = value;
                break;
            case PlanetStatType.ShotCount:
                AttackCount = value;
                break;
        }

        InGameEventHandler.InvokePlanetStateValueChanged(statType, value);
    }

    public void SavePlanetData()
    {
        string json = JsonUtility.ToJson(this);
        PlayerPrefs.SetString("PlanetData", json);
        PlayerPrefs.Save();
    }

    public PlanetData LoadPlanetData()
    {
        string json = PlayerPrefs.GetString("PlanetData", "");
        if (string.IsNullOrEmpty(json))
            return null;

        return JsonUtility.FromJson<PlanetData>(json);
    }
}
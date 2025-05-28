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

public class PlanetMetaData
{
    public int PlanetId { get; private set; }
    public int PlanetLevel { get; private set; }
    public double AttackPower { get; private set; }
    public double AttackCooltime { get; private set; }
    public double AttackSpeed { get; private set; }
    public double Range { get; private set; }
    public double Hp { get; private set; }
    public double HpRecovery { get; private set; }
    public double ShotCount { get; private set; }

    public PlanetMetaData(int planetId, int planetLevel, double attackPower, double attackCooltime,
    double attackSpeed, double range, double hp, double hpRecovery, double shotCount)
    {
        PlanetId = planetId;
        PlanetLevel = planetLevel;
        AttackPower = attackPower;
        AttackCooltime = attackCooltime;
        AttackSpeed = attackSpeed;
        Range = range;
        Hp = hp;
        HpRecovery = hpRecovery;
        ShotCount = shotCount;
    }
}

[Serializable]
public class PlanetData
{
    public PlanetMetaData MetaData { get; private set; }

    public int PlanetId { get => MetaData.PlanetId; }

    public double MaxHp { get; set; }
    public double Hp { get; set; }
    public double HpRecovery { get; set; }
    public double Range { get; set; }
    public double AttackPower { get; set; }
    public double AttackSpeed { get; set; }
    public double AttackCooltime { get; set; }
    public double ShotCount { get; set; }

    public PlanetData(PlanetMetaData metaData)
    {
        MetaData = metaData;

        MaxHp = MetaData.Hp;
        Hp = MetaData.Hp;
        HpRecovery = MetaData.HpRecovery;
        Range = MetaData.Range;
        AttackPower = MetaData.AttackPower;
        AttackSpeed = MetaData.AttackSpeed;
        AttackCooltime = MetaData.AttackCooltime;
        ShotCount = MetaData.ShotCount;
    }

    public double GetStateValue(PlanetStatType statType)
    {
        return statType switch
        {
            PlanetStatType.Hp => Hp,
            PlanetStatType.MaxHp => MaxHp,
            PlanetStatType.HpRecovery => HpRecovery,
            PlanetStatType.Range => Range,
            PlanetStatType.AttackPower => AttackPower,
            PlanetStatType.AttackSpeed => AttackSpeed,
            PlanetStatType.AttackCooltime => AttackCooltime,
            PlanetStatType.ShotCount => ShotCount,
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
                //MaHp 증가량 만큼 Hp 증가
                Hp = Math.Min(Hp + (value - MaxHp), MaxHp);
                MaxHp = value;
                break;
            case PlanetStatType.HpRecovery:
                HpRecovery = value;
                break;
            case PlanetStatType.Range:
                Range = value;
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
                ShotCount = value;
                break;
        }

        InGameEventManager.Instance.InvokePlanetStateValueChanged(statType, value);
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
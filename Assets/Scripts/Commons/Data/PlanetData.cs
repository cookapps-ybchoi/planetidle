using System;
using UnityEngine;

public enum PlanetStatType
{
    PlanetLevel,
    AttackPower,
    AttackSpeed,
    Hp,
    HpRecovery,
    AttackCooltime,
    Range,
}

public class PlanetMetaData
{
    public int PlanetId { get; private set; }
    public int PlanetLevel { get; private set; }
    public double AttackPower { get; private set; }
    public double AttackPowerPerLevel { get; private set; }
    public double AttackCooltime { get; private set; }
    public double AttackSpeed { get; private set; }
    public double AttackSpeedIncreaseRatePerLevel { get; private set; }
    public double Range { get; private set; }
    public double RangePerLevel { get; private set; }
    public double Hp { get; private set; }
    public double HpPerLevel { get; private set; }
    public double HpRecovery { get; private set; }
    public double HpRecoveryPerLevel { get; private set; }

    public PlanetMetaData(int planetId, int planetLevel, double attackPower, double attackPowerPerLevel, double attackCooltime, double attackSpeed, double attackSpeedIncreaseRatePerLevel, double range, double rangePerLevel, double hp, double hpPerLevel, double hpRecovery, double hpRecoveryPerLevel)
    {
        PlanetId = planetId;
        PlanetLevel = planetLevel;
        AttackPower = attackPower;
        AttackPowerPerLevel = attackPowerPerLevel;
        AttackCooltime = attackCooltime;
        AttackSpeed = attackSpeed;
        AttackSpeedIncreaseRatePerLevel = attackSpeedIncreaseRatePerLevel;
        Range = range;
        RangePerLevel = rangePerLevel;
        Hp = hp;
        HpPerLevel = hpPerLevel;
        HpRecovery = hpRecovery;
        HpRecoveryPerLevel = hpRecoveryPerLevel;
    }
}

[Serializable]
public class PlanetData
{
    public PlanetMetaData MetaData { get; private set; }

    public int PlanetId { get => MetaData.PlanetId; }
    private int[] _statLevels;

    public PlanetData(PlanetMetaData metaData)
    {
        MetaData = metaData;
    }

    public void Initialize()
    {
        _statLevels = new int[Enum.GetValues(typeof(PlanetStatType)).Length];
    }

    public int IncreaseStateLevel(PlanetStatType statType)
    {
        int index = (int)statType;
        if (index >= 0 && index < _statLevels.Length)
        {
            _statLevels[index]++;
            return _statLevels[index];
        }
        return 0;
    }

    public int GetStatLevel(PlanetStatType statType)
    {
        int index = (int)statType;
        return index >= 0 && index < _statLevels.Length ? _statLevels[index] : 0;
    }

    public double GetStatValue(PlanetStatType statType)
    {
        return GetStatDefault(statType) + GetStatLevel(statType) * GetStatMultiplier(statType);
    }

    public double GetNextLevelStatValue(PlanetStatType statType)
    {
        return GetStatDefault(statType) + (GetStatLevel(statType) + 1) * GetStatMultiplier(statType);
    }

    private double GetStatDefault(PlanetStatType statType)
    {
        return statType switch
        {
            PlanetStatType.Range => MetaData.Range,
            PlanetStatType.AttackPower => MetaData.AttackPower,
            PlanetStatType.AttackCooltime => MetaData.AttackCooltime,
            PlanetStatType.AttackSpeed => MetaData.AttackSpeed,
            PlanetStatType.Hp => MetaData.Hp,
            PlanetStatType.HpRecovery => MetaData.HpRecovery,
            _ => 0f
        };
    }

    private double GetStatMultiplier(PlanetStatType statType)
    {
        return statType switch
        {
            PlanetStatType.Range => MetaData.RangePerLevel,
            PlanetStatType.AttackPower => MetaData.AttackPowerPerLevel,
            PlanetStatType.AttackSpeed => MetaData.AttackSpeedIncreaseRatePerLevel,
            PlanetStatType.Hp => MetaData.HpPerLevel,
            PlanetStatType.HpRecovery => MetaData.HpRecoveryPerLevel,
            _ => 0
        };
    }

    public void SavePlanetData(PlanetData data)
    {
        string json = JsonUtility.ToJson(data);
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
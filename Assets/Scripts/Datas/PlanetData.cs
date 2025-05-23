public enum PlanetStatType
{
    Level,
    AttackPower,
    AttackCooltime,
    AttackSpeed,
    Range,
    Hp,
    HpRecovery
}

public class PlanetMetaData
{
    public int PlanetId { get; private set; }
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

    public PlanetMetaData(int planetId, double attackPower, double attackPowerPerLevel, double attackCooltime, double attackSpeed, double attackSpeedIncreaseRatePerLevel, double range, double rangePerLevel, double hp, double hpPerLevel, double hpRecovery, double hpRecoveryPerLevel)
    {
        PlanetId = planetId;
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

public class PlanetData
{
    public PlanetMetaData MetaData { get; private set; }

    public int PlanetId { get => MetaData.PlanetId; }
    public int PlanetLevel { get; private set; }
    public int AttackPowerLevel { get; private set; }
    public int AttackSpeedLevel { get; private set; }
    public int RangeLevel { get; private set; }
    public int HpLevel { get; private set; }
    public int HpRecoveryLevel { get; private set; }

    public PlanetData(PlanetMetaData metaData)
    {
        MetaData = metaData;
        PlanetLevel = 0;
        AttackPowerLevel = 0;
        AttackSpeedLevel = 0;
        RangeLevel = 0;
        HpLevel = 0;
        HpRecoveryLevel = 0;
    }

    public void IncreaseLevel(PlanetStatType statType)
    {
        switch (statType)
        {
            case PlanetStatType.Level:
                PlanetLevel++;
                break;
            case PlanetStatType.AttackPower:
                AttackPowerLevel++;
                break;
            case PlanetStatType.AttackSpeed:
                AttackSpeedLevel++;
                break;
            case PlanetStatType.Range:
                RangeLevel++;
                break;
            case PlanetStatType.Hp:
                HpLevel++;
                break;
            case PlanetStatType.HpRecovery:
                HpRecoveryLevel++;
                break;
        }
    }

    public double GetStatValue(PlanetStatType statType)
    {
        return GetStatDefault(statType) + GetStatLevel(statType) * GetStatMultiplier(statType);
    }

    public double GetNextLevelStatValue(PlanetStatType statType)
    {
        return GetStatDefault(statType) + (GetStatLevel(statType) + 1) * GetStatMultiplier(statType);
    }

    private int GetStatLevel(PlanetStatType statType)
    {
        return statType switch
        {
            PlanetStatType.Level => PlanetLevel,
            PlanetStatType.AttackPower => AttackPowerLevel,
            PlanetStatType.AttackSpeed => AttackSpeedLevel,
            PlanetStatType.Range => RangeLevel,
            PlanetStatType.Hp => HpLevel,
            PlanetStatType.HpRecovery => HpRecoveryLevel,
            _ => 0
        };
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
}
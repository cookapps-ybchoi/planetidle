public enum PlanetStatType
{
    Level,
    AttackPower,
    AttackSpeed,
    Range,
    Hp,
    HpRecovery
}

public class PlanetData
{
    public int PlanetId { get; private set; }
    public int PlanetLevel { get; private set; }
    public int AttackPowerLevel { get; private set; }
    public int AttackSpeedLevel { get; private set; }
    public int RangeLevel { get; private set; }
    public int HpLevel { get; private set; }
    public int HpRecoveryLevel { get; private set; }

    public PlanetData(int planetId = 1, int planetLevel = 0, int attackPowerLevel = 0,
        int attackSpeedLevel = 0, int rangeLevel = 0, int hpLevel = 0, int hpRecoveryLevel = 0)
    {
        PlanetId = planetId;
        PlanetLevel = planetLevel;
        AttackPowerLevel = attackPowerLevel;
        AttackSpeedLevel = attackSpeedLevel;
        RangeLevel = rangeLevel;
        HpLevel = hpLevel;
        HpRecoveryLevel = hpRecoveryLevel;
    }

    public void SetData(PlanetData data)
    {
        PlanetId = data.PlanetId;
        PlanetLevel = data.PlanetLevel;
        AttackPowerLevel = data.AttackPowerLevel;
        AttackSpeedLevel = data.AttackSpeedLevel;
        RangeLevel = data.RangeLevel;
        HpLevel = data.HpLevel;
        HpRecoveryLevel = data.HpRecoveryLevel;
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

    public void SetPlanetId(int planetId)
    {
        PlanetId = planetId;
    }

    public float GetStatValue(PlanetStatType statType)
    {
        return GetStatDefault(statType) + GetStatLevel(statType) * GetStatMultiplier(statType);
    }

    public float GetNextLevelStatValue(PlanetStatType statType)
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

    private float GetStatMultiplier(PlanetStatType statType)
    {
        return statType switch
        {
            PlanetStatType.AttackPower => Constants.PLANET_ATTACK_POWER_PER_LEVEL,
            PlanetStatType.AttackSpeed => Constants.PLANET_ATTACK_SPEED_PER_LEVEL,
            PlanetStatType.Range => Constants.PLANET_RANGE_PER_LEVEL,
            PlanetStatType.Hp => Constants.PLANET_HP_PER_LEVEL,
            PlanetStatType.HpRecovery => Constants.PLANET_HP_RECOVERY_PER_LEVEL,
            _ => 1f
        };
    }

    private float GetStatDefault(PlanetStatType statType)
    {
        return statType switch
        {
            PlanetStatType.Range => Constants.PLANET_RANGE_DEFUALT,
            PlanetStatType.AttackPower => Constants.PLANET_ATTACK_POWER_DEFAULT,
            PlanetStatType.AttackSpeed => Constants.PLANET_ATTACK_SPEED_DEFAULT,
            PlanetStatType.Hp => Constants.PLANET_HP_DEFAULT,
            PlanetStatType.HpRecovery => Constants.PLANET_HP_RECOVERY_DEFAULT,
            _ => 0f
        };
    }
}
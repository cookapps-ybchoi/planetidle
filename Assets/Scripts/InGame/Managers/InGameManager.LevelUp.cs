using UnityEngine;
using System.Collections.Generic;

public partial class InGameManager
{
    private const int MAX_ATTACK_POWER_LEVEL = 100;
    private const int MAX_ATTACK_SPEED_LEVEL = 100;
    private const int MAX_HP_LEVEL = 100;
    private const int MAX_HP_RECOVERY_LEVEL = 100;

    private const int MAX_ATTACK_POWER_COST = 10;
    private const int MAX_ATTACK_SPEED_COST = 20;
    private const int MAX_HP_COST = 100;
    private const int MAX_HP_RECOVERY_COST = 100;
    
    private Dictionary<PlanetStatType, int> _maxLevels = new Dictionary<PlanetStatType, int>()
    {
        { PlanetStatType.AttackPower, MAX_ATTACK_POWER_LEVEL },
        { PlanetStatType.AttackSpeed, MAX_ATTACK_SPEED_LEVEL },
        { PlanetStatType.Hp, MAX_HP_LEVEL },
        { PlanetStatType.HpRecovery, MAX_HP_RECOVERY_LEVEL },
    };

    private Dictionary<PlanetStatType, int> _upgradeCosts = new Dictionary<PlanetStatType, int>()
    {
        { PlanetStatType.AttackPower, MAX_ATTACK_POWER_COST },
        { PlanetStatType.AttackSpeed, MAX_ATTACK_SPEED_COST },
        { PlanetStatType.Hp, MAX_HP_COST },
        { PlanetStatType.HpRecovery, MAX_HP_RECOVERY_COST }
    };

    public bool TryUpgradeStat(PlanetStatType statType)
    {
        int currentLevel = GetPlanetStateLevel(statType);
        int maxLevel = GetMaxLevel(statType);
        
        // 최대 레벨 체크
        if (currentLevel >= maxLevel)
        {
            Debug.LogWarning($"{statType}은(는) 이미 최대 레벨({maxLevel})에 도달했습니다.");
            return false;
        }

        int cost = GetUpgradeCost(statType);
        
        if (!TrySpendPoints(cost))
        {
            Debug.LogWarning($"{statType} 레벨 업그레이드 실패: 포인트 부족");
            return false;
        }

        IncreasePlanetStateLevel(statType);
        return true;
    }

    public int GetUpgradeCost(PlanetStatType statType)
    {
        int baseCost = _upgradeCosts[statType];
        int currentLevel = GetPlanetStateLevel(statType);
        return baseCost * (currentLevel + 1); // 레벨이 올라갈수록 비용 증가
    }

    public bool IsMaxLevel(PlanetStatType statType)
    {
        return GetPlanetStateLevel(statType) >= GetMaxLevel(statType);
    }

    public int GetMaxLevel(PlanetStatType statType)
    {
        return _maxLevels[statType];
    }
} 
using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class RecordData
{
    public int TotalEnemiesDestroyed { get; private set; }
    public int TotalPointsEarned { get; private set; }
    public int TotalPointsSpent { get; private set; }
    public Dictionary<PlanetStatType, int> PlanetLevelUps { get; private set; } = new Dictionary<PlanetStatType, int>();

    public void Initialize()
    {
        TotalEnemiesDestroyed = 0;
        TotalPointsEarned = 0;
        TotalPointsSpent = 0;
        PlanetLevelUps.Clear();
    }

    public void RecordEnemyDestroyed()
    {
        TotalEnemiesDestroyed++;
    }

    public void RecordPoints(int points)
    {
        if (points > 0)
        {
            TotalPointsEarned += points;
        }
        else
        {
            TotalPointsSpent += Mathf.Abs(points);
        }
    }

    public void RecordPlanetLevelUp(PlanetStatType statType)
    {
        if (!PlanetLevelUps.ContainsKey(statType))
        {
            PlanetLevelUps[statType] = 0;
        }
        PlanetLevelUps[statType]++;
    }
} 
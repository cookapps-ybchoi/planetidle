using UnityEngine;
using System;
using System.Collections.Generic;

[Serializable]
public class RecordData
{
    public int TotalEnemiesDestroyed { get; private set; }
    public int TotalCoinEarned { get; private set; }
    public int TotalPointsEarned { get; private set; }

    public void Initialize()
    {
        TotalEnemiesDestroyed = 0;
        TotalCoinEarned = 0;
        TotalPointsEarned = 0;
    }

    public void RecordEnemyKilled()
    {
        TotalEnemiesDestroyed++;
    }

    public void RecordPoints(int points)
    {
        TotalPointsEarned += points;
    }

    public void RecordCoins(int coin)
    {
        TotalCoinEarned += coin;
    }
}
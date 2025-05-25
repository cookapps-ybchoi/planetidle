using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InGameManager
{
    private RecordData _recordData = new RecordData();

    public int TotalEnemiesDestroyed => _recordData.TotalEnemiesDestroyed;
    public int TotalPointsEarned => _recordData.TotalPointsEarned;
    public int TotalPointsSpent => _recordData.TotalPointsSpent;

    //행성 레벨업 총 횟수
    public int TotalPlanetLevelUpsCount => _recordData.PlanetLevelUps.Values.Sum();

    //행성 레벨업 개별 횟수
    public int GetPlanetLevelUpCount(PlanetStatType statType)
    {
        return _recordData.PlanetLevelUps[statType];
    }

    private void InitializeRecorder()
    {
        _recordData.Initialize();

        // 이벤트 구독
        InGameEventManager.Instance.OnEnemyDestroyed += OnEnemyDestroyed;
        InGameEventManager.Instance.OnPointsChanged += OnPointsChanged;
        InGameEventManager.Instance.OnPlanetStateLevelChanged += OnPlanetLevelUp;
    }

    private void CleanupRecorder()
    {
        // 이벤트 구독 해제
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnEnemyDestroyed -= OnEnemyDestroyed;
            InGameEventManager.Instance.OnPointsChanged -= OnPointsChanged;
            InGameEventManager.Instance.OnPlanetStateLevelChanged -= OnPlanetLevelUp;
        }
    }

    private void OnEnemyDestroyed(int enemyId)
    {
        _recordData.RecordEnemyDestroyed();
    }

    private void OnPointsChanged(int points, int totalPoints)
    {
        _recordData.RecordPoints(points);
    }

    private void OnPlanetLevelUp(PlanetStatType statType, int level)
    {
        _recordData.RecordPlanetLevelUp(statType);
    }
}
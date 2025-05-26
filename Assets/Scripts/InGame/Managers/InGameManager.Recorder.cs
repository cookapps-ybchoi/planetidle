using UnityEngine;
using System;
using System.Collections.Generic;
using System.Linq;

public partial class InGameManager
{
    private RecordData _recordData = new RecordData();

    public int TotalEnemiesDestroyed => _recordData.TotalEnemiesDestroyed;
    public int TotalCoinEarned => _recordData.TotalCoinEarned;
    public int TotalPointsEarned => _recordData.TotalPointsEarned;

    private void InitializeRecorder()
    {
        _recordData.Initialize();

        // 이벤트 구독
        InGameEventManager.Instance.OnEnemyDestroyed += OnEnemyDestroyed;
        InGameEventManager.Instance.OnCoinEarned += OnCoinEarned;
    }

    private void CleanupRecorder()
    {
        // 이벤트 구독 해제
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnEnemyDestroyed -= OnEnemyDestroyed;
            InGameEventManager.Instance.OnCoinEarned -= OnCoinEarned;
        }
    }

    private void OnEnemyDestroyed(int enemyId)
    {
        _recordData.RecordEnemyDestroyed();
        InGameEventManager.Instance.InvokeRecordDataChanged(_recordData);
    }

    private void OnCoinEarned(int coin)
    {
        _recordData.RecordCoins(coin);
        InGameEventManager.Instance.InvokeRecordDataChanged(_recordData);
    }
}
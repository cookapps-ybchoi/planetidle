using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public enum InGameState
{
    None,
    GamePlay,
    GamePause,
    GameOver,
}

public class InGameEventManager : GameObjectSingleton<InGameEventManager>
{
    public event Action<InGameState> OnGameStateChanged;
    public event Action<int, int> OnPointsChanged;
    public event Action<int> OnCoinEarned;
    public event Action<PlanetStatType, int> OnPlanetStateLevelChanged;
    public event Action<PlanetStatType, double> OnPlanetStateValueChanged;
    public event Action<int> OnEnemyDestroyed;
    public event Action<int> OnEliteMonsterSpawned;
    public event Action<int> OnBossWaveStarted;
    public event Action<int> OnBossWaveCompleted;
    public event Action<float> OnTimeChanged;
    public event Action<RecordData> OnRecordDataChanged;
    public event Action<int> OnLevelChanged;
    public async Task Initialize()
    {
        await Task.CompletedTask;
    }

    public void InvokeGameStateChanged(InGameState state) => OnGameStateChanged?.Invoke(state);
    public void InvokePointsChanged(int currentPoints, int maxPoints) => OnPointsChanged?.Invoke(currentPoints, maxPoints);
    public void InvokeLevelChanged(int level) => OnLevelChanged?.Invoke(level);
    public void InvokePlanetStateLevelChanged(PlanetStatType statType, int level) => OnPlanetStateLevelChanged?.Invoke(statType, level);
    public void InvokePlanetStateValueChanged(PlanetStatType statType, double value) => OnPlanetStateValueChanged?.Invoke(statType, value);
    public void InvokeEnemyDestroyed(int enemyId) => OnEnemyDestroyed?.Invoke(enemyId);
    public void InvokeEliteMonsterSpawned(int waveLevel) => OnEliteMonsterSpawned?.Invoke(waveLevel);
    public void InvokeBossWaveStarted(int waveLevel) => OnBossWaveStarted?.Invoke(waveLevel);
    public void InvokeBossWaveCompleted(int waveLevel) => OnBossWaveCompleted?.Invoke(waveLevel);
    public void InvokeTimeChanged(float totalPlayTime) => OnTimeChanged?.Invoke(totalPlayTime);
    public void InvokeRecordDataChanged(RecordData recordData) => OnRecordDataChanged?.Invoke(recordData);
}

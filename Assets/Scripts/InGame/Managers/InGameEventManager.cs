using UnityEngine;
using System;
using System.Threading.Tasks;
using System.Collections.Generic;

public enum InGameState
{
    None,
    GameReady,
    GamePlay,
    GamePause,
    GameOver,
}

public class InGameEventManager : GameObjectSingleton<InGameEventManager>
{
    public event Action<InGameState> OnGameStateChanged;
    public event Action<int, int> OnExpChanged;
    public event Action<int> OnCoinEarned;
    public event Action<PlanetStatType, int> OnPlanetStateLevelChanged;
    public event Action<PlanetStatType, double> OnPlanetStateValueChanged;
    public event Action<int, bool> OnEnemyDestroyed;
    public event Action<int> OnEliteMonsterSpawned;
    public event Action<int> OnBossWaveStarted;
    public event Action<int> OnBossWaveCompleted;
    public event Action<float> OnTimeChanged;
    public event Action<RecordData> OnRecordDataChanged;
    public event Action<int> OnLevelChanged;
    public event Action<int> OnChoiceSkill;
    public void InvokeGameStateChanged(InGameState state) => OnGameStateChanged?.Invoke(state);
    public void InvokeExpChanged(int currentExp, int maxExp) => OnExpChanged?.Invoke(currentExp, maxExp);
    public void InvokeCoinEarned(int amount) => OnCoinEarned?.Invoke(amount);
    public void InvokeLevelChanged(int level) => OnLevelChanged?.Invoke(level);
    public void InvokePlanetStateLevelChanged(PlanetStatType statType, int level) => OnPlanetStateLevelChanged?.Invoke(statType, level);
    public void InvokePlanetStateValueChanged(PlanetStatType statType, double value) => OnPlanetStateValueChanged?.Invoke(statType, value);
    public void InvokeEnemyDestroyed(int enemyId, bool isKilled) => OnEnemyDestroyed?.Invoke(enemyId, isKilled);
    public void InvokeEliteMonsterSpawned(int waveLevel) => OnEliteMonsterSpawned?.Invoke(waveLevel);
    public void InvokeBossWaveStarted(int waveLevel) => OnBossWaveStarted?.Invoke(waveLevel);
    public void InvokeBossWaveCompleted(int waveLevel) => OnBossWaveCompleted?.Invoke(waveLevel);
    public void InvokeTimeChanged(float totalPlayTime) => OnTimeChanged?.Invoke(totalPlayTime);
    public void InvokeRecordDataChanged(RecordData recordData) => OnRecordDataChanged?.Invoke(recordData);
    public void InvokeChoiceSkill(int skillId) => OnChoiceSkill?.Invoke(skillId);
    public async Task Initialize()
    {
        await Task.CompletedTask;
    }
}

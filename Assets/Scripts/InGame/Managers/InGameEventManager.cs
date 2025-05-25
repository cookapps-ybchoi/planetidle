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
    public event Action<int> OnWaveStart;
    public event Action<float> OnWaveProgressChanged;
    public event Action<float> OnWaveWaitProgressChanged;
    public event Action<int> OnWaveComplete;
    public event Action<int, int> OnPointsChanged;
    public event Action<PlanetStatType, int> OnPlanetStateLevelChanged;
    public event Action<PlanetStatType, double> OnPlanetStateValueChanged;
    public event Action<int> OnEnemyDestroyed;

    public async Task Initialize()
    {
        await Task.CompletedTask;
    }

    public void InvokeGameStateChanged(InGameState state) => OnGameStateChanged?.Invoke(state);
    public void InvokeWaveStart(int waveLevel) => OnWaveStart?.Invoke(waveLevel);
    public void InvokeWaveProgressChanged(float progress) => OnWaveProgressChanged?.Invoke(progress);
    public void InvokeWaveWaitProgressChanged(float progress) => OnWaveWaitProgressChanged?.Invoke(progress);
    public void InvokeWaveComplete(int waveLevel) => OnWaveComplete?.Invoke(waveLevel);
    public void InvokePointsChanged(int points, int totalPoints) => OnPointsChanged?.Invoke(points, totalPoints);
    public void InvokePlanetStateLevelChanged(PlanetStatType statType, int level) => OnPlanetStateLevelChanged?.Invoke(statType, level);
    public void InvokePlanetStateValueChanged(PlanetStatType statType, double value) => OnPlanetStateValueChanged?.Invoke(statType, value);
    public void InvokeEnemyDestroyed(int enemyId) => OnEnemyDestroyed?.Invoke(enemyId);
}

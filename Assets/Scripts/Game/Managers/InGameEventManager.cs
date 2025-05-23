using System;
using System.Threading.Tasks;

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
    public event Action<int> OnWaveComplete;

    public async Task Initialize()
    {
        await Task.CompletedTask;
    }

    public void InvokeGameStateChanged(InGameState state) => OnGameStateChanged?.Invoke(state);
    public void InvokeWaveStart(int waveLevel) => OnWaveStart?.Invoke(waveLevel);
    public void InvokeWaveProgressChanged(float progress) => OnWaveProgressChanged?.Invoke(progress);
    public void InvokeWaveComplete(int waveLevel) => OnWaveComplete?.Invoke(waveLevel);
}

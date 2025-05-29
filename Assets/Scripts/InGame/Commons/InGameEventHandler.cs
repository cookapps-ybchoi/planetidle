using System;
using System.Threading.Tasks;

public enum InGameState
{
    None,
    GameReady,
    GamePlay,
    GamePause,
    GameOver,
}

public class InGameEventHandler
{
    public static event Action<InGameState> OnGameStateChanged;
    public static event Action<int, int> OnExpChanged;
    public static event Action<int> OnCoinEarned;
    public static event Action<PlanetStatType, double> OnPlanetStateValueChanged;
    public static event Action<int, bool> OnEnemyDestroyed;
    public static event Action<int> OnEliteMonsterSpawned;
    public static event Action<int> OnBossWaveStarted;
    public static event Action<int> OnBossWaveCompleted;
    public static event Action<float> OnTimeChanged;
    public static event Action<RecordData> OnRecordDataChanged;
    public static event Action<int> OnLevelChanged;
    public static event Action<InGameSkillId> OnChoiceSkill;


    public static void InvokeGameStateChanged(InGameState state) => OnGameStateChanged?.Invoke(state);
    public static void InvokeExpChanged(int currentExp, int maxExp) => OnExpChanged?.Invoke(currentExp, maxExp);
    public static void InvokeCoinEarned(int amount) => OnCoinEarned?.Invoke(amount);
    public static void InvokeLevelChanged(int level) => OnLevelChanged?.Invoke(level);
    public static void InvokePlanetStateValueChanged(PlanetStatType statType, double value) => OnPlanetStateValueChanged?.Invoke(statType, value);
    public static void InvokeEnemyDestroyed(int enemyId, bool isKilled) => OnEnemyDestroyed?.Invoke(enemyId, isKilled);
    public static void InvokeEliteMonsterSpawned(int waveLevel) => OnEliteMonsterSpawned?.Invoke(waveLevel);
    public static void InvokeBossWaveStarted(int waveLevel) => OnBossWaveStarted?.Invoke(waveLevel);
    public static void InvokeBossWaveCompleted(int waveLevel) => OnBossWaveCompleted?.Invoke(waveLevel);
    public static void InvokeTimeChanged(float totalPlayTime) => OnTimeChanged?.Invoke(totalPlayTime);
    public static void InvokeRecordDataChanged(RecordData recordData) => OnRecordDataChanged?.Invoke(recordData);
    public static void InvokeChoiceSkill(InGameSkillId skillName) => OnChoiceSkill?.Invoke(skillName);
}

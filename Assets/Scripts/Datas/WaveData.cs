public class WaveData
{
    public int WaveId { get; private set; }
    public int WaveLevel { get; private set; }
    public int SpawnCount { get; private set; }
    public float SpawnInterval { get; private set; }
    public float[] SpawnRates { get; private set; }

    public WaveData(int waveId, int waveLevel, int spawnCount, float spawnInterval, float[] spawnRates)
    {
        WaveId = waveId;
        WaveLevel = waveLevel;
        SpawnCount = spawnCount;
        SpawnInterval = spawnInterval;
        SpawnRates = spawnRates;
    }
}
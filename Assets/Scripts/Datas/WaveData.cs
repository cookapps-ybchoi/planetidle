public class WaveData
{
    public int WaveId { get; private set; }
    public int WaveLevel { get; private set; }
    public int SpawnCount { get; private set; }
    
    //동시 생산 갯수
    public int BatchCount { get; private set; }

    public float SpawnInterval { get; private set; }
    public int[] SpawnIds { get; private set; }
    public float[] SpawnRates { get; private set; }

    public int TotalSpawnCount => SpawnCount * BatchCount;

    public WaveData(int waveId, int waveLevel, int spawnCount, int batchCount, float spawnInterval, int[] spawnIds, float[] spawnRates)
    {
        WaveId = waveId;
        WaveLevel = waveLevel;
        SpawnCount = spawnCount;
        BatchCount = batchCount;
        SpawnInterval = spawnInterval;
        SpawnIds = spawnIds;
        SpawnRates = spawnRates;
    }
}
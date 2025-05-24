public class WaveMetaData
{
    public int WaveId { get; private set; }
    public int WaveLevel { get; private set; }
    public int SpawnCount { get; private set; }
    
    //동시 생산 갯수
    public int BatchCount { get; private set; }

    //생산 간격
    public float SpawnInterval { get; private set; }

    //생산 아이디
    public int SpawnId { get; private set; }

    //생산 확률
    public float SpawnRate { get; private set; }

    public int TotalSpawnCount => SpawnCount * BatchCount;

    public WaveMetaData(int waveId, int waveLevel, int spawnCount, int batchCount, float spawnInterval, int spawnId, float spawnRate)
    {
        WaveId = waveId;
        WaveLevel = waveLevel;
        SpawnCount = spawnCount;
        BatchCount = batchCount;
        SpawnInterval = spawnInterval;
        SpawnId = spawnId;
        SpawnRate = spawnRate;
    }
}
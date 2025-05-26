public class WaveMetaData
{
    public int WaveId { get; private set; }
    public int WaveLevel { get; private set; }
    public int SpawnCount { get; private set; }

    //생산 간격
    public float SpawnInterval { get; private set; }

    //생산 아이디
    public int SpawnId { get; private set; }
    //생산 확률
    public float SpawnRate { get; private set; }

    public WaveMetaData(int waveId, int waveLevel, int spawnCount,float spawnInterval, int spawnId, float spawnRate)
    {
        WaveId = waveId;
        WaveLevel = waveLevel;
        SpawnCount = spawnCount;
        SpawnInterval = spawnInterval;
        SpawnId = spawnId;
        SpawnRate = spawnRate;
    }
}
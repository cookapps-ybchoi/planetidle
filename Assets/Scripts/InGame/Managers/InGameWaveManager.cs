using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using System.Collections;
using UnityEngine.Pool;

public class InGameWaveManager : GameObjectSingleton<InGameWaveManager>
{
    private List<InGameEnemy> _enemies = new();
    private int _currentWaveLevel = 1;
    private int _currentSpawnCount = 0;
    private int _totalSpawnCount = 0;
    private float _waveWaitTime = 0f;
    private int _enemySpawnId = 0;

    public async Task Initialize()
    {
        _currentWaveLevel = 1;
        _enemySpawnId = 0;
        await Task.CompletedTask;
    }

    public int CurrentWave => _currentWaveLevel;

    //현재 웨이브의 진행률 리턴
    public float GetCurrentWaveProgress()
    {
        if (_totalSpawnCount <= 0)
        {
            return 0f;
        }
        return (float)_currentSpawnCount / (float)_totalSpawnCount;
    }

    public float GetCurrentWaveWaitProgress()
    {
        return _waveWaitTime / Constants.WAVE_INTERVAL;
    }

    //웨이브 시작
    //웨이브 데이터가 없으면 가장 마지막 웨이브 반복
    public void StartWave(int waveLevel = 1)
    {
        if (InGameManager.Instance.IsPlaying)
        {
            _currentWaveLevel = waveLevel;
            List<WaveMetaData> waveDatas = DataManager.Instance.WaveDataList.Where(data => data.WaveLevel == _currentWaveLevel).ToList();
            if (waveDatas.Count == 0)
            {
                //웨이브 데이터가 없으면 가장 마지막 웨이브 반복
                var lastWaveLevel = DataManager.Instance.WaveDataList.Max(data => data.WaveLevel);
                waveDatas.AddRange(DataManager.Instance.WaveDataList.Where(data => data.WaveLevel == lastWaveLevel));
            }
            InGameEventManager.Instance.InvokeWaveStart(_currentWaveLevel);
            StartCoroutine(SpawnEnemies(waveDatas));
        }
    }

    public void StopWave()
    {
        //전체 적 제거
        foreach (var enemy in _enemies)
        {
            enemy.Finish();
        }
        _enemies.Clear();
    }

    private IEnumerator SpawnEnemies(List<WaveMetaData> waveDatas)
    {
        _currentSpawnCount = 0;
        _totalSpawnCount = waveDatas.First().TotalSpawnCount;
        int spawnCount = waveDatas.First().SpawnCount;
        int batchCount = waveDatas.First().BatchCount;
        float spawnInterval = waveDatas.First().SpawnInterval;

        for (int i = 0; i < spawnCount; i++)
        {
            for (int j = 0; j < batchCount; j++)
            {
                WaveMetaData selectedWaveData = GetRandomWaveData(waveDatas);
                if (selectedWaveData != null)
                {
                    int randomEnemyId = selectedWaveData.SpawnId;
                    StartCoroutine(SpawnEnemyCoroutine(randomEnemyId));
                    _currentSpawnCount++;
                    InGameEventManager.Instance.InvokeWaveProgressChanged(GetCurrentWaveProgress());
                }
            }

            yield return new WaitForSeconds(spawnInterval);

            if (InGameManager.Instance.IsPlaying == false) yield break;
        }

        InGameEventManager.Instance.InvokeWaveComplete(_currentWaveLevel);
        StartCoroutine(WaitForNextWave());
    }

    // 웨이브 완료 후 다음 웨이브 시작 사이 대기
    private IEnumerator WaitForNextWave()
    {
        //웨이브 대기 진행률을 이벤트로 전달
        _waveWaitTime = 0f;
        while (_waveWaitTime < Constants.WAVE_INTERVAL)
        {
            _waveWaitTime += Time.deltaTime;
            InGameEventManager.Instance.InvokeWaveWaitProgressChanged(_waveWaitTime / Constants.WAVE_INTERVAL);
            yield return null;
        }

        if (InGameManager.Instance.IsPlaying == false) yield break;

        StartWave(_currentWaveLevel + 1);
    }

    private IEnumerator SpawnEnemyCoroutine(int enemyId)
    {
        float distance = Constants.ENEMY_SPAWN_DISTANCE;
        float randomAngleRadians = UnityEngine.Random.Range(0, 360) * Mathf.Deg2Rad;
        Vector3 spawnPosition = InGameManager.Instance.Planet.transform.position +
            new Vector3(Mathf.Cos(randomAngleRadians), Mathf.Sin(randomAngleRadians), 0) * distance;

        var enemyTask = AddressableManager.Instance.GetEnemy(enemyId, spawnPosition, transform);
        yield return new WaitUntil(() => enemyTask.IsCompleted);
        var enemy = enemyTask.Result;
        EnemyData enemyData = new EnemyData(DataManager.Instance.EnemyDataList.Find(data => data.EnemyId == enemyId), _currentWaveLevel);
        enemy.Initialize(enemyData, _enemySpawnId);
        _enemies.Add(enemy);
        _enemySpawnId++;
    }

    private WaveMetaData GetRandomWaveData(List<WaveMetaData> waveDatas)
    {
        float totalRate = waveDatas.Sum(data => data.SpawnRate);
        float randomValue = UnityEngine.Random.Range(0f, totalRate);
        float currentSum = 0f;

        foreach (var waveData in waveDatas)
        {
            currentSum += waveData.SpawnRate;
            if (randomValue <= currentSum)
            {
                return waveData;
            }
        }

        return null;
    }

    public void RemoveEnemy(InGameEnemy enemy)
    {
        _enemies.Remove(enemy);
    }

    public InGameEnemy GetTargetEnemy(Vector3 position, double range)
    {
        InGameEnemy closestEnemy = null;
        float closestDistanceSquared = float.MaxValue;
        double rangeSquared = range * range;

        // 거리 기반 필터링을 먼저 수행
        var nearbyEnemies = _enemies.Where(e => e != null &&
            (e.transform.position - position).sqrMagnitude <= rangeSquared);

        foreach (var enemy in nearbyEnemies)
        {
            Vector3 direction = enemy.transform.position - position;
            float distanceSquared = direction.sqrMagnitude;
            float actualDistance = Mathf.Sqrt(distanceSquared) - enemy.EnemySize;

            if (actualDistance <= range && actualDistance < Mathf.Sqrt(closestDistanceSquared))
            {
                closestEnemy = enemy;
                closestDistanceSquared = distanceSquared;
            }
        }
        return closestEnemy;
    }
}

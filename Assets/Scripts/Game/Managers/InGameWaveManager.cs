using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using UnityEditor;
using System;
using System.Collections;

public class InGameWaveManager : GameObjectSingleton<InGameWaveManager>
{
    private List<InGameEnemy> _enemies = new List<InGameEnemy>();
    private WaveData _waveData = null;
    private int _currentWaveLevel = 1;
    private int _currentSpawnCount = 0;
    private float _totalSpawnRate = 0f;

    public async Task Initialize()
    {
        _currentWaveLevel = 1;
        await Task.CompletedTask;
    }

    //현재 웨이브의 진행률 리턴
    public float GetCurrentWaveProgress()
    {
        if (_waveData == null)
        {
            return 0f;
        }
        return (float)_currentSpawnCount / (float)_waveData.TotalSpawnCount;
    }

    //웨이브 시작
    //웨이브 데이터가 없으면 가장 마지막 웨이브 반복
    public void StartWave(int waveLevel = 1)
    {
        if (InGameManager.Instance.IsPlaying)
        {
            _currentWaveLevel = waveLevel;
            _waveData = DataManager.Instance.WaveDataList.Find(data => data.WaveLevel == _currentWaveLevel);
            if (_waveData == null)
            {
                //웨이브 데이터가 없으면 가장 마지막 웨이브 반복
                _waveData = DataManager.Instance.WaveDataList.Last();
            }

            // SpawnRates 합산 미리 계산
            _totalSpawnRate = _waveData.SpawnRates.Sum();

            InGameEventManager.Instance.InvokeWaveStart(_currentWaveLevel);
            StartCoroutine(SpawnEnemies(_waveData));
        }
    }

    public void StopWave()
    {
        _waveData = null;
        //전체 적 제거
        foreach (var enemy in _enemies)
        {
            enemy.Finish();
        }
        _enemies.Clear();
    }

    private IEnumerator SpawnEnemies(WaveData waveData)
    {
        _currentSpawnCount = 0;

        for (int i = 0; i < waveData.SpawnCount; i++)
        {
            for (int j = 0; j < waveData.BatchCount; j++)
            {
                int randomEnemyId = GetRandomEnemyId(waveData);
                StartCoroutine(SpawnEnemyCoroutine(randomEnemyId));
                _currentSpawnCount++;

                InGameEventManager.Instance.InvokeWaveProgressChanged(GetCurrentWaveProgress());
            }

            yield return new WaitForSeconds(waveData.SpawnInterval);

            if (InGameManager.Instance.IsPlaying == false) yield break;
        }

        InGameEventManager.Instance.InvokeWaveComplete(_currentWaveLevel);

        yield return new WaitForSeconds(Constants.WAVE_INTERVAL);

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
        enemy.Initialize(enemyData);
        _enemies.Add(enemy);
    }

    private int GetRandomEnemyId(WaveData waveData)
    {
        float randomValue = UnityEngine.Random.Range(0f, _totalSpawnRate);
        float currentSum = 0f;

        for (int k = 0; k < waveData.SpawnRates.Length; k++)
        {
            currentSum += waveData.SpawnRates[k];
            if (randomValue <= currentSum)
            {
                return waveData.SpawnIds[k];
            }
        }

        return waveData.SpawnIds[0];
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

    //GIZMO로 현재 진행레벨과 진행률을 텍스트로 표시
    private void OnDrawGizmos()
    {
        if (_waveData == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Handles.Label(transform.position + Vector3.up, $"Wave {_currentWaveLevel} : {GetCurrentWaveProgress() * 100:F1}%");
    }
}

using System.Collections.Generic;
using System.Threading.Tasks;
using UnityEngine;
using System.Linq;
using UnityEditor;

public class InGameWaveManager : GameObjectSingleton<InGameWaveManager>
{
    private List<InGameEnemy> _enemies = new List<InGameEnemy>();
    private WaveData _waveData = null;
    private int _currentWaveLevel = 1;
    private int _currentSpawnCount = 0;

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

    public void StartWave()
    {
        _waveData = DataManager.Instance.WaveDataList.Find(data => data.WaveLevel == _currentWaveLevel);
        if (_waveData == null)
        {
            //웨이브 데이터가 없으면 가장 마지막 웨이브 반복
            _waveData = DataManager.Instance.WaveDataList.Last();
        }

        // 총 생산 횟수, 생산간격, 적타입(ID)
        SpawnEnemies(_waveData);
    }

    // 정해진 규칙동안 적을 생산하는 함수
    // 파라미터, 생산 횟수, 생산간격, 적타입(ID)
    private async void SpawnEnemies(WaveData waveData)
    {
        _currentSpawnCount = 0;

        // SpawnRates의 전체 합산 계산
        float totalRate = 0f;
        foreach (float rate in waveData.SpawnRates)
        {
            totalRate += rate;
        }

        // 생산 횟수만큼 적을 생산하는 함수
        for (int i = 0; i < waveData.SpawnCount; i++)
        {
            //BatchCount만큼 적을 생산
            for (int j = 0; j < waveData.BatchCount; j++)
            {
                // 랜덤 적 생성, SpawnRates 비율에 따라 적 생성
                float randomValue = Random.Range(0f, totalRate);
                float currentSum = 0f;
                int randomEnemyId = 1; // 기본값으로 첫 번째 적 설정

                for (int k = 0; k < waveData.SpawnRates.Length; k++)
                {
                    currentSum += waveData.SpawnRates[k];
                    if (randomValue <= currentSum)
                    {
                        randomEnemyId = waveData.SpawnIds[j];
                        break;
                    }
                }

                InGameEnemy enemy = await SpawnEnemy(randomEnemyId);
                enemy.SetData(DataManager.Instance.EnemyDataList.Find(data => data.EnemyId == randomEnemyId), waveData.WaveLevel);
                _enemies.Add(enemy);
                _currentSpawnCount++;
            }
            await Task.Delay((int)(waveData.SpawnInterval * 1000));
        }

        //웨이브가 끝나면 인터벌 만큼 기다렸다가 다음 웨이브 시작
        await Task.Delay((int)(Constants.WAVE_INTERVAL * 1000));
        _currentWaveLevel++;
        StartWave();
    }

    public void RemoveEnemy(InGameEnemy enemy)
    {
        _enemies.Remove(enemy);
    }

    public InGameEnemy GetTargetEnemy(Vector3 position, float range)
    {
        InGameEnemy closestEnemy = null;
        float closestDistanceSquared = float.MaxValue;

        foreach (var enemy in _enemies)
        {
            if (enemy == null) continue;

            Vector3 direction = enemy.transform.position - position;
            float distanceSquared = direction.sqrMagnitude;

            // 적의 크기를 고려한 실제 거리 계산
            float actualDistance = Mathf.Sqrt(distanceSquared) - enemy.EnemySize;

            // 실제 거리가 범위 내에 있는지 확인
            if (actualDistance <= range && actualDistance < Mathf.Sqrt(closestDistanceSquared))
            {
                closestEnemy = enemy;
                closestDistanceSquared = distanceSquared;
            }
        }
        return closestEnemy;
    }

    private async Task<InGameEnemy> SpawnEnemy(int enemyId)
    {

        // 적 생성, 위치는 행성에서 특정 거리만큼 떨어진 랜덤 위치
        float distance = Constants.ENEMY_SPAWN_DISTANCE;
        float randomAngleRadians = Random.Range(0, 360) * Mathf.Deg2Rad;
        Vector3 spawnPosition = InGameManager.Instance.GetPlanetTransform().position +
            new Vector3(Mathf.Cos(randomAngleRadians), Mathf.Sin(randomAngleRadians), 0) * distance;
        return await AddressableManager.Instance.GetEnemy(enemyId, spawnPosition, transform);
    }

    //GIZMO로 현재 진행레벨과 진행률을 텍스트로 표시
    private void OnDrawGizmos()
    {
        if (_waveData == null)
        {
            return;
        }
        Gizmos.color = Color.green;
        Handles.Label(transform.position + Vector3.up, $"Wave {_currentWaveLevel} : {GetCurrentWaveProgress() * 100}%");
    }
}

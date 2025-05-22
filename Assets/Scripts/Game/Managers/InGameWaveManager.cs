using System.Threading.Tasks;
using UnityEngine;

public class InGameWaveManager : GameObjectSingleton<InGameWaveManager>
{
    private int _currentWaveLevel = 1;
    private int _currentWaveCount = 0;

    public void Initialize()
    {
        _currentWaveLevel = 1;
        _currentWaveCount = 0;
    }

    public void StartWave()
    {
        WaveData waveData = DataManager.Instance.WaveDataList.Find(data => data.WaveLevel == _currentWaveLevel);

        // 총 생산 횟수, 생산간격, 적타입(ID)
        SpawnEnemies(waveData);
    }

    // 정해진 규칙동안 적을 생산하는 함수
    // 파라미터, 생산 횟수, 생산간격, 적타입(ID)
    private async void SpawnEnemies(WaveData waveData)
    {
        Debug.Log($"Wave {waveData.WaveLevel} 시작 : {waveData.SpawnCount}번 생산, 간격 {waveData.SpawnInterval}초");

        // SpawnRates의 전체 합산 계산
        float totalRate = 0f;
        foreach (float rate in waveData.SpawnRates)
        {
            totalRate += rate;
        }

        // 생산 횟수만큼 적을 생산하는 함수
        for (int i = 0; i < waveData.SpawnCount; i++)
        {
            // 랜덤 적 생성, SpawnRates 비율에 따라 적 생성
            float randomValue = Random.Range(0f, totalRate);
            float currentSum = 0f;
            int randomEnemyId = 1; // 기본값으로 첫 번째 적 설정

            for (int j = 0; j < waveData.SpawnRates.Length; j++)
            {
                currentSum += waveData.SpawnRates[j];
                if (randomValue <= currentSum)
                {
                    randomEnemyId = j + 1;
                    break;
                }
            }

            InGameEnemy enemy = await SpawnEnemy(randomEnemyId);
            enemy.Initialize();
            enemy.SetData(DataManager.Instance.EnemyDataList.Find(data => data.EnemyId == randomEnemyId));
            await Task.Delay((int)(waveData.SpawnInterval * 1000));
        }
    }

    private async Task<InGameEnemy> SpawnEnemy(int enemyId)
    {

        // 적 생성, 위치는 행성에서 특정 거리만큼 떨어진 랜덤 위치
        float distance = Constants.ENEMY_SPAWN_DISTANCE;
        float randomAngleRadians = Random.Range(0, 360) * Mathf.Deg2Rad;
        Vector3 spawnPosition = InGameManager.Instance.GetPlanetTransform().position +
            new Vector3(Mathf.Cos(randomAngleRadians), Mathf.Sin(randomAngleRadians), 0) * distance;
        Debug.Log($"적 {enemyId} 생성 위치 : {spawnPosition}");
        return await AddressableManager.Instance.GetEnemy(enemyId, spawnPosition, transform);
    }
}

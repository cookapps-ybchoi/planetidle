using System.Threading.Tasks;
using UnityEngine;

public class EnemySpawnManager : GameObjectSingleton<EnemySpawnManager>
{
    public void Initialize()
    {

    }

    public void StartLevelSpawn(int level = 1)
    {
        //임시 데이터
        int totalCount = 10;
        float interval = 2f;
        int enemyId = 1;

        // 총 생산 횟수, 생산간격, 적타입(ID)
        SpawnEnemies(totalCount, interval, enemyId);
    }

    // 정해진 규칙동안 적을 생산하는 함수
    // 파라미터, 생산 횟수, 생산간격, 적타입(ID)
    private async void SpawnEnemies(int count, float interval, int enemyId)
    {
        // 생산 횟수만큼 적을 생산하는 함수
        
        for (int i = 0; i < count; i++)
        {
            InGameEnemy enemy = await SpawnEnemy();
            await Task.Delay((int)(interval * 1000));
        }
    }
    private async Task<InGameEnemy> SpawnEnemy()
    {
        // 적 생성, 위치는 행성에서 특정 거리만큼 떨어진 랜덤 위치
        float distance = Constants.ENEMY_SPAWN_DISTANCE;
        float randomAngle = Random.Range(0, 360);
        Vector3 spawnPosition = InGameManager.Instance.GetPlanetTransform().position + new Vector3(Mathf.Cos(randomAngle), Mathf.Sin(randomAngle), 0) * distance;
        InGameEnemy enemy = await AddressableManager.Instance.GetEnemy(1, spawnPosition, transform);
        enemy.Initialize();
        return enemy;
    }
}

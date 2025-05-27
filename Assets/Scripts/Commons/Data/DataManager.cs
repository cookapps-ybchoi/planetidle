using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataManager : GameObjectSingleton<DataManager>
{
    private PlanetData _planetData;
    private List<EnemyMetaData> _enemyDatas;
    private List<WaveMetaData> _waveDatas;
    private string _planetSavePath => Path.Combine(Application.persistentDataPath, "planetData.json");

    public PlanetData PlanetData => _planetData;
    public List<EnemyMetaData> EnemyDataList => _enemyDatas;
    public List<WaveMetaData> WaveDataList => _waveDatas;

    public async Task Initialize()
    {
        _planetData = await LoadPlanetDataAsync();
        _enemyDatas = LoadEnemyMetaDatas();
        _waveDatas = LoadWaveMetaDatas();
    }

    public async Task SaveAsync()
    {
        await SavePlanetDataAsync();
    }

    private PlanetData CreatePlanetData()
    {
        _planetData = new PlanetData(new PlanetMetaData(planetId: 1, planetLevel: 1,
        attackPower: Constants.PLANET_ATTACK_POWER_DEFAULT, attackPowerPerLevel: Constants.PLANET_ATTACK_POWER_PER_LEVEL,
        attackCooltime: Constants.PLANET_ATTACK_COOLTIME_DEFUALT,
        attackSpeed: Constants.PLANET_ATTACK_SPEED_DEFAULT, attackSpeedIncreaseRatePerLevel: Constants.PLANET_ATTACK_SPEED_INCREASE_RATE_PER_LEVEL,
        range: Constants.PLANET_RANGE_DEFUALT, rangePerLevel: Constants.PLANET_RANGE_PER_LEVEL,
        hp: Constants.PLANET_HP_DEFAULT, hpPerLevel: Constants.PLANET_HP_PER_LEVEL,
        hpRecovery: Constants.PLANET_HP_RECOVERY_DEFAULT, hpRecoveryPerLevel: Constants.PLANET_HP_RECOVERY_PER_LEVEL));
        return _planetData;
    }

    private async Task SavePlanetDataAsync()
    {
        if (_planetData == null)
        {
            Debug.LogWarning("저장할 데이터가 없습니다.");
            return;
        }

        try
        {
            string jsonData = JsonUtility.ToJson(_planetData, true);
            await File.WriteAllTextAsync(_planetSavePath, jsonData);
            Debug.Log($"데이터가 성공적으로 저장되었습니다: {_planetSavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"데이터 저장 중 오류 발생: {e.Message}");
        }
    }

    private async Task<PlanetData> LoadPlanetDataAsync()
    {
        if (!File.Exists(_planetSavePath))
        {
            Debug.Log("저장된 데이터가 없습니다. 새로운 데이터를 생성합니다.");
            return CreatePlanetData();
        }

        try
        {
            string jsonData = await File.ReadAllTextAsync(_planetSavePath);
            _planetData = JsonUtility.FromJson<PlanetData>(jsonData);
            Debug.Log("데이터를 성공적으로 불러왔습니다.");
            return _planetData;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"데이터 로드 중 오류 발생: {e.Message}");
            return CreatePlanetData();
        }
    }

    private List<EnemyMetaData> LoadEnemyMetaDatas()
    {
        List<EnemyMetaData> enemyMetaDatas = new List<EnemyMetaData>();
        // 일반 비행기
        enemyMetaDatas.Add(new EnemyMetaData(1, EnemyType.Normal, hp: 8, moveSpeed: 0.8f, attackRange: 0.3f, attackPower: 1f, attackDelay: 1f, point: 1, hpIncRate: 0.1f));    // 기본
        enemyMetaDatas.Add(new EnemyMetaData(2, EnemyType.Normal, hp: 8, moveSpeed: 1.2f, attackRange: 0.3f, attackPower: 1f, attackDelay: 1f, point: 1, hpIncRate: 0.1f));    // 빠른 속도
        enemyMetaDatas.Add(new EnemyMetaData(3, EnemyType.Normal, hp: 20, moveSpeed: 0.5f, attackRange: 0.3f, attackPower: 1f, attackDelay: 1f, point: 2, hpIncRate: 0.1f));   // 높은 HP
        
        // 엘리트 비행기
        enemyMetaDatas.Add(new EnemyMetaData(101, EnemyType.Elite, hp: 50, moveSpeed: 0.3f, attackRange: 0.3f, attackPower: 2f, attackDelay: 1f, point: 5, hpIncRate: 0.1f));   // 엘리트 1
        enemyMetaDatas.Add(new EnemyMetaData(102, EnemyType.Elite, hp: 50, moveSpeed: 0.3f, attackRange: 0.3f, attackPower: 2f, attackDelay: 1f, point: 5, hpIncRate: 0.1f));   // 엘리트 2
        
        // 보스 비행기
        enemyMetaDatas.Add(new EnemyMetaData(201, EnemyType.Boss, hp: 100, moveSpeed: 0.2f, attackRange: 0.3f, attackPower: 2f, attackDelay: 1f, point: 50, hpIncRate: 0.1f));   // 보스

        return enemyMetaDatas;
    }

    private List<WaveMetaData> LoadWaveMetaDatas()
    {
        List<WaveMetaData> waveMetaDatas = new List<WaveMetaData>();
        // 웨이브 데이터 생성 (임시)
        // 웨이브 1 초당 1개 1초 간격 생성, 분당 60개 생성
        waveMetaDatas.Add(new WaveMetaData(1, waveLevel: 1, spawnCount: 1, spawnInterval: 1f, spawnId: 1, spawnRate: 1.0f));
        // 웨이브 2 초당 2개 0.75초 간격 생성, 분당 120개 생성
        waveMetaDatas.Add(new WaveMetaData(2, waveLevel: 2, spawnCount: 1, spawnInterval: 0.75f, spawnId: 1, spawnRate: 0.8f));
        waveMetaDatas.Add(new WaveMetaData(3, waveLevel: 2, spawnCount: 1, spawnInterval: 0.75f, spawnId: 2, spawnRate: 0.2f));
        // 웨이브 3 초당 1개 0.75초 간격 생성, 분당 180개 생성
        waveMetaDatas.Add(new WaveMetaData(4, waveLevel: 3, spawnCount: 1, spawnInterval: 0.75f, spawnId: 1, spawnRate: 0.9f));
        waveMetaDatas.Add(new WaveMetaData(5, waveLevel: 3, spawnCount: 1, spawnInterval: 0.75f, spawnId: 2, spawnRate: 0.9f));
        waveMetaDatas.Add(new WaveMetaData(6, waveLevel: 3, spawnCount: 1, spawnInterval: 0.75f, spawnId: 3, spawnRate: 0.1f));
        // 웨이브 4 초당 2개 0.75초 간격 생성, 분당 240개 생성
        waveMetaDatas.Add(new WaveMetaData(7, waveLevel: 4, spawnCount: 2, spawnInterval: 0.75f, spawnId: 1, spawnRate: 0.9f));
        waveMetaDatas.Add(new WaveMetaData(8, waveLevel: 4, spawnCount: 2, spawnInterval: 0.75f, spawnId: 2, spawnRate: 0.25f));
        waveMetaDatas.Add(new WaveMetaData(9, waveLevel: 4, spawnCount: 2, spawnInterval: 0.75f, spawnId: 3, spawnRate: 0.15f));
        // 웨이브 5 초당 2개 0.5초 간격 생성, 분당 300개 생성
        waveMetaDatas.Add(new WaveMetaData(10, waveLevel: 5, spawnCount: 2, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.8f));
        waveMetaDatas.Add(new WaveMetaData(11, waveLevel: 5, spawnCount: 2, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.8f));
        waveMetaDatas.Add(new WaveMetaData(12, waveLevel: 5, spawnCount: 2, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.2f));
        // 웨이브 6 초당 3개 0.5초 간격 생성, 분당 360개 생성
        waveMetaDatas.Add(new WaveMetaData(13, waveLevel: 6, spawnCount: 3, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.8f));
        waveMetaDatas.Add(new WaveMetaData(14, waveLevel: 6, spawnCount: 3, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.8f));
        waveMetaDatas.Add(new WaveMetaData(15, waveLevel: 6, spawnCount: 3, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.2f));
        // 웨이브 7 초당 3개 0.5초 간격 생성, 분당 360개 생성
        waveMetaDatas.Add(new WaveMetaData(16, waveLevel: 7, spawnCount: 3, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.7f));
        waveMetaDatas.Add(new WaveMetaData(17, waveLevel: 7, spawnCount: 3, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.7f));
        waveMetaDatas.Add(new WaveMetaData(18, waveLevel: 7, spawnCount: 3, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.25f));
        // 웨이브 8
        waveMetaDatas.Add(new WaveMetaData(19, waveLevel: 8, spawnCount: 4, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.5f));
        waveMetaDatas.Add(new WaveMetaData(20, waveLevel: 8, spawnCount: 4, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.7f));
        waveMetaDatas.Add(new WaveMetaData(21, waveLevel: 8, spawnCount: 4, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.3f));
        // 웨이브 9
        waveMetaDatas.Add(new WaveMetaData(22, waveLevel: 9, spawnCount: 4, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.4f));
        waveMetaDatas.Add(new WaveMetaData(23, waveLevel: 9, spawnCount: 4, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.4f));
        waveMetaDatas.Add(new WaveMetaData(24, waveLevel: 9, spawnCount: 4, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.3f));
        // 웨이브 10
        waveMetaDatas.Add(new WaveMetaData(25, waveLevel: 10, spawnCount: 5, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.4f));
        waveMetaDatas.Add(new WaveMetaData(26, waveLevel: 10, spawnCount: 5, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.6f));
        waveMetaDatas.Add(new WaveMetaData(27, waveLevel: 10, spawnCount: 5, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.4f));
        // 웨이브 11
        waveMetaDatas.Add(new WaveMetaData(28, waveLevel: 11, spawnCount: 5, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.4f));
        waveMetaDatas.Add(new WaveMetaData(29, waveLevel: 11, spawnCount: 5, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.6f));
        waveMetaDatas.Add(new WaveMetaData(30, waveLevel: 11, spawnCount: 5, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.4f));
        // 웨이브 12
        waveMetaDatas.Add(new WaveMetaData(31, waveLevel: 12, spawnCount: 6, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.4f));
        waveMetaDatas.Add(new WaveMetaData(32, waveLevel: 12, spawnCount: 6, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.6f));
        waveMetaDatas.Add(new WaveMetaData(33, waveLevel: 12, spawnCount: 6, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.4f));
        // 웨이브 13
        waveMetaDatas.Add(new WaveMetaData(34, waveLevel: 13, spawnCount: 6, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.4f));
        waveMetaDatas.Add(new WaveMetaData(35, waveLevel: 13, spawnCount: 6, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.6f));
        waveMetaDatas.Add(new WaveMetaData(36, waveLevel: 13, spawnCount: 6, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.4f));
        // 웨이브 14
        waveMetaDatas.Add(new WaveMetaData(37, waveLevel: 14, spawnCount: 7, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.4f));
        waveMetaDatas.Add(new WaveMetaData(38, waveLevel: 14, spawnCount: 7, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.6f));
        waveMetaDatas.Add(new WaveMetaData(39, waveLevel: 14, spawnCount: 7, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.4f));
        // 웨이브 15
        waveMetaDatas.Add(new WaveMetaData(40, waveLevel: 15, spawnCount: 7, spawnInterval: 0.5f, spawnId: 1, spawnRate: 0.4f));
        waveMetaDatas.Add(new WaveMetaData(41, waveLevel: 15, spawnCount: 7, spawnInterval: 0.5f, spawnId: 2, spawnRate: 0.6f));
        waveMetaDatas.Add(new WaveMetaData(42, waveLevel: 15, spawnCount: 7, spawnInterval: 0.5f, spawnId: 3, spawnRate: 0.4f));
        return waveMetaDatas;
    }

    public async Task DeleteSaveDataAsync()
    {
        if (File.Exists(_planetSavePath))
        {
            try
            {
                await Task.Run(() => File.Delete(_planetSavePath));
                Debug.Log("저장된 데이터가 삭제되었습니다.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"데이터 삭제 중 오류 발생: {e.Message}");
            }
        }
    }
}

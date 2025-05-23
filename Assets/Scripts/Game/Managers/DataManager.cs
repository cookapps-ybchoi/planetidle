using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataManager : GameObjectSingleton<DataManager>
{
    private PlanetData _planetData;
    private List<EnemyMetaData> _enemyDatas;
    private List<WaveData> _waveDatas;
    private string _planetSavePath => Path.Combine(Application.persistentDataPath, "planetData.json");

    public PlanetData PlanetData => _planetData;
    public List<EnemyMetaData> EnemyDataList => _enemyDatas;
    public List<WaveData> WaveDataList => _waveDatas;

    public async Task Initialize()
    {
        _planetData = await LoadPlanetDataAsync();
        _enemyDatas = LoadEnemyMetaDatas();
        _waveDatas = LoadWaveDatas();
    }

    public async Task SaveAsync()
    {
        await SavePlanetDataAsync();
    }

    private PlanetData CreatePlanetData()
    {
        _planetData = new PlanetData(new PlanetMetaData(1, 
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
        // 적 데이터 생성 (임시)
        enemyMetaDatas.Add(new EnemyMetaData(1, EnemyType.Normal, hp: 10, moveSpeed: 0.8f, attackRange: 0.3f, attackPower: 1f, attackDelay: 1f, point: 2, pointPerLevel: 2));    // 기본
        enemyMetaDatas.Add(new EnemyMetaData(2, EnemyType.Normal, hp: 8, moveSpeed: 1.5f, attackRange: 0.3f, attackPower: 1f, attackDelay: 1f, point: 2, pointPerLevel: 2));    // 빠른 속도
        enemyMetaDatas.Add(new EnemyMetaData(3, EnemyType.Normal, hp: 30, moveSpeed: 0.5f, attackRange: 0.3f, attackPower: 1f, attackDelay: 1f, point: 2, pointPerLevel: 2));   // 높은 HP
        return enemyMetaDatas;
    }

    private List<WaveData> LoadWaveDatas()
    {
        List<WaveData> waveDatas = new List<WaveData>();
        // 웨이브 데이터 생성 (임시)
        waveDatas.Add(new WaveData(1, 1, 20, 1, 1f, new int[] { 1, 2, 3 }, new float[] { 1.0f, 0.0f, 0.0f }));
        waveDatas.Add(new WaveData(2, 2, 20, 1, 1f, new int[] { 1, 2, 3 }, new float[] { 0.8f, 0.2f, 0.0f }));
        waveDatas.Add(new WaveData(3, 3, 20, 1, 0.9f, new int[] { 1, 2, 3 }, new float[] { 0.7f, 0.2f, 0.1f }));
        waveDatas.Add(new WaveData(4, 4, 20, 1, 0.9f, new int[] { 1, 2, 3 }, new float[] { 0.6f, 0.25f, 0.15f }));
        waveDatas.Add(new WaveData(5, 5, 20, 1, 0.8f, new int[] { 1, 2, 3 }, new float[] { 0.6f, 0.2f, 0.2f }));
        waveDatas.Add(new WaveData(6, 6, 20, 2, 0.8f, new int[] { 1, 2, 3 }, new float[] { 0.5f, 0.3f, 0.2f }));
        waveDatas.Add(new WaveData(7, 7, 20, 2, 0.7f, new int[] { 1, 2, 3 }, new float[] { 0.5f, 0.25f, 0.25f }));
        waveDatas.Add(new WaveData(8, 8, 20, 2, 0.7f, new int[] { 1, 2, 3 }, new float[] { 0.5f, 0.2f, 0.3f }));
        waveDatas.Add(new WaveData(9, 9, 20, 2, 0.6f, new int[] { 1, 2, 3 }, new float[] { 0.4f, 0.4f, 0.3f }));
        waveDatas.Add(new WaveData(10, 10, 20, 2, 0.6f, new int[] { 1, 2, 3 }, new float[] { 0.4f, 0.3f, 0.4f }));
        return waveDatas;
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

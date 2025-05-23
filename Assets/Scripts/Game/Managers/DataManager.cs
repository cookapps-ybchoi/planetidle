using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataManager : GameObjectSingleton<DataManager>
{
    private PlanetData _planetData;
    private List<EnemyData> _enemyDatas;
    private List<WaveData> _waveDatas;
    private string _planetSavePath => Path.Combine(Application.persistentDataPath, "planetData.json");

    public PlanetData PlanetData => _planetData;
    public List<EnemyData> EnemyDataList => _enemyDatas;
    public List<WaveData> WaveDataList => _waveDatas;

    public async Task Initialize()
    {
        _planetData = await LoadPlanetDataAsync();
        _enemyDatas = LoadEnemyDatas();
        _waveDatas = LoadWaveDatas();
    }

    public async Task SaveAsync()
    {
        await SavePlanetDataAsync();
    }

    private PlanetData CreatePlanetData()
    {
        _planetData = new PlanetData();
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

    private List<EnemyData> LoadEnemyDatas()
    {
        List<EnemyData> enemyDatas = new List<EnemyData>();
        // 적 데이터 생성 (임시)
        enemyDatas.Add(new EnemyData(1, EnemyType.Normal, 1, 10, 0.6f, 0.3f, 1f, 2f));    // 기본
        enemyDatas.Add(new EnemyData(2, EnemyType.Normal, 1, 8, 1.2f, 0.3f, 1f, 2f));    // 빠른 속도
        enemyDatas.Add(new EnemyData(3, EnemyType.Normal, 1, 30, 0.3f, 0.3f, 1f, 2f));   // 높은 HP
        return enemyDatas;
    }

    private List<WaveData> LoadWaveDatas()
    {
        List<WaveData> waveDatas = new List<WaveData>();
        // 웨이브 데이터 생성 (임시)
        waveDatas.Add(new WaveData(1, 1, 10, 1, 1.5f, new int[] { 1, 2, 3 }, new float[] { 1.0f, 0.0f, 0.0f }));
        waveDatas.Add(new WaveData(2, 2, 20, 1, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.8f, 0.2f, 0.0f }));
        waveDatas.Add(new WaveData(3, 3, 20, 1, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.7f, 0.2f, 0.1f }));
        waveDatas.Add(new WaveData(4, 4, 20, 1, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.6f, 0.25f, 0.15f }));
        waveDatas.Add(new WaveData(5, 5, 20, 1, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.6f, 0.2f, 0.2f }));
        waveDatas.Add(new WaveData(6, 6, 20, 2, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.5f, 0.3f, 0.2f }));
        waveDatas.Add(new WaveData(7, 7, 20, 2, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.5f, 0.25f, 0.25f }));
        waveDatas.Add(new WaveData(8, 8, 20, 2, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.5f, 0.2f, 0.3f }));
        waveDatas.Add(new WaveData(9, 9, 20, 2, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.4f, 0.4f, 0.3f }));
        waveDatas.Add(new WaveData(10, 10, 20, 2, 1.5f, new int[] { 1, 2, 3 }, new float[] { 0.4f, 0.3f, 0.4f }));
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

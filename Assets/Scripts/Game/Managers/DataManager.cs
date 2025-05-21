using UnityEngine;
using System.IO;

public class DataManager : GameObjectSingleton<DataManager>
{
    private PlanetData _planetData;
    private string _planetSavePath => Path.Combine(Application.persistentDataPath, "planetData.json");

    public PlanetData PlanetData => _planetData;

    public void Init()
    {
        _planetData = LoadPlanetData();
    }

    public void Save()
    {
        SavePlanetData();
    }

    private PlanetData CreatePlanetData()
    {
        _planetData = new PlanetData();
        return _planetData;
    }

    private void SavePlanetData()
    {
        if (_planetData == null)
        {
            Debug.LogWarning("저장할 데이터가 없습니다.");
            return;
        }

        try
        {
            string jsonData = JsonUtility.ToJson(_planetData, true);
            File.WriteAllText(_planetSavePath, jsonData);
            Debug.Log($"데이터가 성공적으로 저장되었습니다: {_planetSavePath}");
        }
        catch (System.Exception e)
        {
            Debug.LogError($"데이터 저장 중 오류 발생: {e.Message}");
        }
    }

    private PlanetData LoadPlanetData()
    {
        if (!File.Exists(_planetSavePath))
        {
            Debug.Log("저장된 데이터가 없습니다. 새로운 데이터를 생성합니다.");
            return CreatePlanetData();
        }

        try
        {
            string jsonData = File.ReadAllText(_planetSavePath);
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

    public void DeleteSaveData()
    {
        if (File.Exists(_planetSavePath))
        {
            try
            {
                File.Delete(_planetSavePath);
                Debug.Log("저장된 데이터가 삭제되었습니다.");
            }
            catch (System.Exception e)
            {
                Debug.LogError($"데이터 삭제 중 오류 발생: {e.Message}");
            }
        }
    }
}

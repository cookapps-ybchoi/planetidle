using UnityEngine;
using System.IO;
using System.Collections.Generic;
using System.Threading.Tasks;

public class DataManager : GameObjectSingleton<DataManager>
{
    private string _planetSavePath => Path.Combine(Application.persistentDataPath, "planetData.json");

    [SerializeField] private Enemies _enemies;
    [SerializeField] private Waves _waves;

    private List<InGameSkillMetaData> _inGameSkillDatas;

    public List<EnemyEntity> EnemyDataList => _enemies.Entities;
    public List<WaveEntity> WaveDataList => _waves.Entities;
    public List<InGameSkillMetaData> InGameSkillDataList => _inGameSkillDatas;

    public Task Initialize()
    {
        _inGameSkillDatas = LoadInGameSkillMetaDatas();
        return Task.CompletedTask;
    }

    public async Task SaveAsync(PlanetData planetData)
    {
        await SavePlanetDataAsync(planetData);
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

    public PlanetData CreatePlanetData()
    {
        return new PlanetData(new PlanetMetaData(planetId: 1, planetLevel: 1,
        attackPower: Constants.PLANET_ATTACK_POWER_DEFUALT,
        attackCooltime: Constants.PLANET_ATTACK_COOLTIME_DEFUALT,
        range: Constants.PLANET_RANGE_DEFUALT,
        attackSpeed: Constants.PLANET_ATTACK_SPEED_DEFUALT,
        hp: Constants.PLANET_HP_DEFAULT,
        hpRecovery: Constants.PLANET_HP_RECOVERY_DEFAULT,
        shotCount: Constants.PLANET_SHOT_COUNT_DEFAULT));
    }

    private async Task SavePlanetDataAsync(PlanetData planetData)
    {
        if (planetData == null)
        {
            Debug.LogWarning("저장할 데이터가 없습니다.");
            return;
        }

        try
        {
            string jsonData = JsonUtility.ToJson(planetData, true);
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
            PlanetData planetData = JsonUtility.FromJson<PlanetData>(jsonData);
            Debug.Log("데이터를 성공적으로 불러왔습니다.");
            return planetData;
        }
        catch (System.Exception e)
        {
            Debug.LogError($"데이터 로드 중 오류 발생: {e.Message}");
            return CreatePlanetData();
        }
    }

    private List<InGameSkillMetaData> LoadInGameSkillMetaDatas()
    {
        List<InGameSkillMetaData> inGameSkillMetaDatas = new List<InGameSkillMetaData>();

        inGameSkillMetaDatas.Add(new InGameSkillMetaData(
            name: "Multi Shot",
            description: "Attack multiple enemies simultaneously.",
            id: InGameSkillId.MultiShot,
            type: InGameSkillType.Passive,
            maxLevel: 5,
            firstValue: 1,
            secondValue: 0));

        inGameSkillMetaDatas.Add(new InGameSkillMetaData(
            name: "Rapid Fire",
            description: "Enhance your attack speed to strike faster.",
            id: InGameSkillId.AttackSpeed,
            type: InGameSkillType.Passive,
            maxLevel: 5,
            firstValue: 0.1,
            secondValue: 0));

        inGameSkillMetaDatas.Add(new InGameSkillMetaData(
            name: "Power Surge",
            description: "Amplify your attack power to deal devastating damage.",
            id: InGameSkillId.AttackPower,
            type: InGameSkillType.Passive,
            maxLevel: 5,
            firstValue: 0.2,
            secondValue: 0));

        inGameSkillMetaDatas.Add(new InGameSkillMetaData(
            name: "Attack Range",
            description: "Increase your attack range.",
            id: InGameSkillId.AttackRange,
            type: InGameSkillType.Passive,
            maxLevel: 5,
            firstValue: 0.1,
            secondValue: 0));

        inGameSkillMetaDatas.Add(new InGameSkillMetaData(
            name: "Health Boost",
            description: "Increase your maximum health.",
            id: InGameSkillId.Hp,
            type: InGameSkillType.Passive,
            maxLevel: 5,
            firstValue: 0.5,
            secondValue: 0));

        return inGameSkillMetaDatas;
    }
}

using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class InGameManager : GameObjectSingleton<InGameManager>
{
    private InGamePlanet _planet;

    public Transform GetPlanetTransform()
    {
        return _planet.transform;
    }

    private async void Start()
    {
        // DataManager 인스턴스 대기
        await WaitForInstance(DataManager.Instance);

        // 데이터 초기화
        await DataManager.Instance.Initialize();

        // ObjectPoolManager 인스턴스 대기
        await WaitForInstance(ObjectPoolManager.Instance);

        // ObjectPoolManager 초기화
        await ObjectPoolManager.Instance.Initialize();

        // AddressableManager 인스턴스 대기
        await WaitForInstance(AddressableManager.Instance);

        // AddressableManager 초기화
        await AddressableManager.Instance.Initialize();

        // InGameWaveManager 인스턴스 대기
        await WaitForInstance(InGameWaveManager.Instance);

        // InGameWaveManager 초기화
        InGameWaveManager.Instance.Initialize();

        // 행성 생성
        await CreatePlanet();

        // 미리 로드
        await Preload();

        // 레벨 시작
        StartGame();
    }

    public void StartGame()
    {
        InGameWaveManager.Instance.StartWave();
    }

    public double GetPlanetAttackPower()
    {
        return DataManager.Instance.PlanetData.GetStatValue(PlanetStatType.AttackPower);
    }

    private async Task CreatePlanet()
    {
        _planet = await AddressableManager.Instance.GetPlanet(DataManager.Instance.PlanetData.PlanetId, Vector3.zero, transform);
        _planet.Initialize();
    }

    private async Task Preload()
    {
        InGameEnemy enemy = await AddressableManager.Instance.GetEnemy(1, Vector3.zero, transform);
        enemy.gameObject.SetActive(false);

        InGameBullet bullet = await AddressableManager.Instance.GetBullet(1, Vector3.zero, transform);
        bullet.gameObject.SetActive(false);

        InGameExplosion explosion = await AddressableManager.Instance.GetExplosion(1, Vector3.zero, transform);
        explosion.gameObject.SetActive(false);
    }

    private async Task WaitForInstance<T>(T instance) where T : class
    {
        while (instance == null) await Task.Yield();
    }
}


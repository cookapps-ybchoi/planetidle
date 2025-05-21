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
        DataManager.Instance.Initialize();

        // AddressableManager 인스턴스 대기
        await WaitForInstance(AddressableManager.Instance);

        // 행성 프리팹 인스턴스화
        _planet = await AddressableManager.Instance.GetPlanet(DataManager.Instance.PlanetData.PlanetId, Vector3.zero, transform);

        if (_planet != null)
        {
            _planet.Initialize();
        }

        // 적 생성 매니저 인스턴스 대기
        await WaitForInstance(InGameWaveManager.Instance);

        // 적 생성 매니저 초기화
        InGameWaveManager.Instance.Initialize();


        // 레벨 시작
        StartGame();
    }

    public void StartGame()
    {
        InGameWaveManager.Instance.StartWave();
    }

    private async Task WaitForInstance<T>(T instance) where T : class
    {
        while (instance == null) await Task.Yield();
    }
}


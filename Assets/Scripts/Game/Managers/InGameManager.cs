using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;
using Unity.VisualScripting;

public class InGameManager : GameObjectSingleton<InGameManager>
{
    [SerializeField] private PrefabAddressConfig prefabConfig;

    private InGamePlanet _planet;

    private async void Start()
    {
        // DataManager 인스턴스 대기
        await WaitForInstance(DataManager.Instance);

        // 데이터 초기화
        DataManager.Instance.Init();

        // AddressableManager 인스턴스 대기
        await WaitForInstance(AddressableManager.Instance);

        // 행성 프리팹 인스턴스화
        _planet = await AddressableManager.Instance.InstantiateAsync<InGamePlanet>(
            prefabConfig.InGamePlanet, Vector3.zero, transform);

        if (_planet != null)
        {
            _planet.Initialize();
        }
    }

    private async Task WaitForInstance<T>(T instance) where T : class
    {
        while (instance == null) await Task.Yield();
    }
}


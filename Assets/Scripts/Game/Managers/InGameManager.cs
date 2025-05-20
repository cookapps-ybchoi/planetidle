using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class InGameManager : MonoBehaviour
{
    [SerializeField] private PrefabAddressConfig prefabConfig;

    private async void Start()
    {
        // GoblinEnemy.cs 컴포넌트가 붙은 프리팹이라고 가정
        InGamePlanet planet = await AddressableManager.Instance.InstantiateAsync<InGamePlanet>(
            prefabConfig.InGamePlanet, Vector3.zero, transform);

        if (planet != null)
        {
            planet.Initialize(); // 원하는 초기화 로직 실행
        }
    }
}


using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class AddressableManager : GameObjectSingleton<AddressableManager>
{
    [SerializeField] private PrefabAddressConfig prefabConfig;

    /// <summary>
    /// 파라미터로 행성 아이디를 받아 행성 프리펩을 생성
    /// </summary>
    public async Task<InGamePlanet> GetPlanet(int planetId, Vector3 position = default, Transform parent = null)
    {
        //아이디는 3자리 숫자로 표시
        string address = $"{prefabConfig.InGamePlanet}_{planetId:D3}";
        return await InstantiateAsync<InGamePlanet>(address, position, parent);
    }

    public async Task<InGameEnemy> GetEnemy(int enemyId, Vector3 position = default, Transform parent = null)
    {
        string address = $"{prefabConfig.InGameEnemy}_{enemyId:D3}";
        return await InstantiateAsync<InGameEnemy>(address, position, parent);
    }

    public async Task<InGameBullet> GetBullet(int bulletId, Vector3 position = default, Transform parent = null)
    {
        string address = $"{prefabConfig.InGameBullet}_{bulletId:D3}";
        return await InstantiateAsync<InGameBullet>(address, position, parent);
    }

    /// <summary>
    /// 주소로부터 오브젝트를 생성하고 T 컴포넌트를 반환
    /// </summary>
    private async Task<T> InstantiateAsync<T>(string address, Vector3 position = default, Transform parent = null) where T : Component
    {
        AsyncOperationHandle<GameObject> handle = Addressables.LoadAssetAsync<GameObject>(address);
        await handle.Task;

        if (handle.Status == AsyncOperationStatus.Succeeded)
        {
            GameObject obj = Instantiate(handle.Result, position, Quaternion.identity, parent);
            T component = obj.GetComponent<T>();
            if (component == null)
            {
                Debug.LogWarning($"[{address}]에 {typeof(T).Name} 컴포넌트가 없습니다.");
            }
            return component;
        }
        else
        {
            Debug.LogError($"Failed to load addressable at: {address}");
            return null;
        }
    }
}

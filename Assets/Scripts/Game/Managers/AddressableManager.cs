using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class AddressableManager : GameObjectSingleton<AddressableManager>
{
    public PrefabAddressConfig PrefabAddressConfig => prefabConfig;

    [SerializeField] private PrefabAddressConfig prefabConfig;

    public async Task Initialize()
    {
        await Task.CompletedTask;
    }

    /// <summary>
    /// 제네릭 메서드를 사용하여 게임 오브젝트를 풀에서 가져옵니다.
    /// </summary>
    /// <typeparam name="T">가져올 게임 오브젝트의 타입</typeparam>
    /// <param name="objectType">오브젝트 타입 (예: InGamePlanet, InGameEnemy 등)</param>
    /// <param name="objectId">오브젝트 ID</param>
    /// <param name="position">생성 위치</param>
    /// <param name="parent">부모 Transform</param>
    /// <returns>생성된 게임 오브젝트</returns>
    public async Task<T> GetGameObject<T>(string objectType, int objectId, Vector3 position = default, Transform parent = null) where T : Component
    {
        string address = $"{objectType}_{objectId:D3}";
        return await ObjectPoolManager.Instance.GetFromPool<T>(address, position, parent);
    }

    /// <summary>
    /// 행성 오브젝트를 풀에서 가져옵니다.
    /// </summary>
    public async Task<InGamePlanet> GetPlanet(int planetId, Vector3 position = default, Transform parent = null)
    {
        return await GetGameObject<InGamePlanet>(prefabConfig.InGamePlanet, planetId, position, parent);
    }

    /// <summary>
    /// 적 오브젝트를 풀에서 가져옵니다.
    /// </summary>
    public async Task<InGameEnemy> GetEnemy(int enemyId, Vector3 position = default, Transform parent = null)
    {
        return await GetGameObject<InGameEnemy>(prefabConfig.InGameEnemy, enemyId, position, parent);
    }

    /// <summary>
    /// 총알 오브젝트를 풀에서 가져옵니다.
    /// </summary>
    public async Task<InGameBullet> GetBullet(int bulletId, Vector3 position = default, Transform parent = null)
    {
        return await GetGameObject<InGameBullet>(prefabConfig.InGameBullet, bulletId, position, parent);
    }

    public async Task<InGameExplosion> GetExplosion(int explosionId, Vector3 position = default, Transform parent = null)
    {
        return await GetGameObject<InGameExplosion>(prefabConfig.InGameExplosion, explosionId, position, parent);
    }

    /// <summary>
    /// 오브젝트를 풀로 반환합니다.
    /// </summary>
    public void ReturnToPool(GameObject obj)
    {
        ObjectPoolManager.Instance.ReturnToPool(obj.name, obj);
    }
}

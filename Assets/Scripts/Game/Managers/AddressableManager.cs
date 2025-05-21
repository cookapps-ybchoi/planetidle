using UnityEngine;
using UnityEngine.AddressableAssets;
using UnityEngine.ResourceManagement.AsyncOperations;
using System.Threading.Tasks;

public class AddressableManager : GameObjectSingleton<AddressableManager>
{
    /// <summary>
    /// 주소로부터 오브젝트를 생성하고 T 컴포넌트를 반환
    /// </summary>
    public async Task<T> InstantiateAsync<T>(string address, Vector3 position = default, Transform parent = null) where T : Component
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

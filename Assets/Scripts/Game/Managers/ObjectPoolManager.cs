using UnityEngine;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ObjectPoolManager : GameObjectSingleton<ObjectPoolManager>
{
    private readonly Dictionary<string, Queue<GameObject>> _poolDictionary = new();
    private readonly Dictionary<string, GameObject> _prefabDictionary = new();
    private readonly Dictionary<string, int> _poolSizes = new();

    [SerializeField] private readonly int _defaultPoolSize = 20;

    public async Task<T> GetFromPool<T>(string address, Vector3 position = default, Transform parent = null) where T : Component
    {
        if (!_poolDictionary.ContainsKey(address))
        {
            await InitializePool(address);
        }

        if (_poolDictionary[address].Count == 0)
        {
            await ExpandPool(address);
        }

        GameObject obj = _poolDictionary[address].Dequeue();
        obj.name = address;
        obj.transform.position = position;
        obj.transform.SetParent(parent);
        obj.SetActive(true);

        return obj.GetComponent<T>();
    }

    public void ReturnToPool(string address, GameObject obj)
    {
        if (!_poolDictionary.ContainsKey(address))
        {
            Debug.LogWarning($"Pool for {address} doesn't exist!");
            return;
        }

        obj.SetActive(false);
        obj.transform.SetParent(transform);
        _poolDictionary[address].Enqueue(obj);
    }

    private async Task InitializePool(string address)
    {
        var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<GameObject>(address);
        await handle.Task;

        if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
        {
            _prefabDictionary[address] = handle.Result;
            _poolDictionary[address] = new Queue<GameObject>();
            _poolSizes[address] = _defaultPoolSize;

            for (int i = 0; i < _defaultPoolSize; i++)
            {
                CreateNewInstance(address);
            }
        }
        else
        {
            Debug.LogError($"Failed to load addressable at: {address}");
        }
    }

    private Task ExpandPool(string address)
    {
        int newSize = _poolSizes[address] * 2;
        _poolSizes[address] = newSize;

        for (int i = 0; i < _poolSizes[address]; i++)
        {
            CreateNewInstance(address);
        }
        return Task.CompletedTask;
    }

    private void CreateNewInstance(string address)
    {
        GameObject obj = Instantiate(_prefabDictionary[address], transform);
        obj.SetActive(false);
        _poolDictionary[address].Enqueue(obj);
    }
}
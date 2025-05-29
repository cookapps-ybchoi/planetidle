using UnityEngine;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;

public class ImageLoader
{
    private static Dictionary<string, Sprite> _iconImageCache = new Dictionary<string, Sprite>();

    public static async Task<Sprite> LoadIcon(InGameSkillId skillId)
    {
        //skillId 의 int 값 3자리로 변환 skill_icon_001
        string iconName = $"{Constants.INGAME_SKILL_ICON_NAME_PREFIX}{((int)skillId).ToString("D3")}";
        return await LoadIcon(iconName);
    }

    public static async Task<Sprite> LoadIcon(string iconName)
    {
        // 이미 캐싱된 이미지가 있는지 확인
        Sprite cachedSprite = GetCachedIcon(iconName);
        if (cachedSprite != null)
        {
            return cachedSprite;
        }

        try
        {
            string address = $"{AddressableManager.Instance.PrefabAddressConfig.SkillIcon}/{iconName}";
            var handle = UnityEngine.AddressableAssets.Addressables.LoadAssetAsync<Sprite>(address);
            await handle.Task;

            if (handle.Status == UnityEngine.ResourceManagement.AsyncOperations.AsyncOperationStatus.Succeeded)
            {
                Sprite sprite = handle.Result;
                if (sprite != null)
                {
                    _iconImageCache[iconName] = sprite;
                    return sprite;
                }
                else
                {
                    Debug.LogError($"Failed to load icon: {iconName}");
                    return null;
                }
            }
            else
            {
                Debug.LogError($"Failed to load icon: {iconName}");
                return null;
            }
        }
        catch (Exception e)
        {
            Debug.LogError($"Error loading icon {iconName}: {e.Message}");
            return null;
        }
    }

    private static Sprite GetCachedIcon(string iconName)
    {
        if (_iconImageCache.TryGetValue(iconName, out Sprite sprite))
        {
            return sprite;
        }
        return null;
    }

    public static void ClearCache()
    {
        _iconImageCache.Clear();
    }
}

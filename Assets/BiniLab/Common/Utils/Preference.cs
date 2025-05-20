using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Pref
{
    LANGUAGE,
    BGM_V,
    SFX_V,
    SPEED_MODE,
    SHOW_INTRO,
    VOX_V,
    SAFE_MODE_T,
    USE_SAFE_MODE,
    AUTO_SKILL,
    FOV,
    SPEC_VERSION,
    SPEC_DATA,
    LAN_SYSTEM_VERSION,
    LAN_SYSTEM_SAVE,
    LAN_SYSTEM_DATA,
    LAN_SCENARIO_VERSION,
    LAN_SCENARIO_SAVE,
    LAN_SCENARIO_DATA,
    GUEST_ID,
    SOCIAL_ID,
    DATA_TIME,
    EXIST_POST,
    CHAT_IGNORE,
    USE_PUSH,
    SHOW_FX,
    SHOW_DAMAGE,
    DECLARATION,
    CHAT_BAN,
    FRIEND_HISTORY_DATA,
    PVP_MATCH_LIST,
    QUIZ,
    FIRST_PLAY
}

public class Preference
{
    public static List<T> LoadListPreference<T>(Pref pref)
    {
        List<T> ret = null;
        try
        {
            string json = UnityEngine.PlayerPrefs.GetString(pref.ToString());

            if (string.IsNullOrEmpty(json))
            {
                return new List<T>();
            }

            ret = JsonUtility.FromJson<Wrapper<T>>(json).Items;
        }
        catch
        {
            return new List<T>();
        }

        return ret;
    }

    public static List<int> LoadPreference(Pref pref, List<int> defaultValue)
    {
        string json = UnityEngine.PlayerPrefs.GetString(pref.ToString());
        Wrapper<int> wrapper = JsonUtility.FromJson<Wrapper<int>>(json);
        if (wrapper == null || wrapper.Items == null) return defaultValue;
        return new List<int>(wrapper.Items);
    }

    public static void SavePreference<T>(Pref pref, List<T> value)
    {
        Wrapper<T> wrapper = new Wrapper<T> { Items = value };
        string json = JsonUtility.ToJson(wrapper);
        UnityEngine.PlayerPrefs.SetString(pref.ToString(), json);
        UnityEngine.PlayerPrefs.Save();
    }

    public static void SavePreference(Pref pref, List<int> value)
    {
        Wrapper<int> wrapper = new Wrapper<int> { Items = value };
        string json = JsonUtility.ToJson(wrapper);
        UnityEngine.PlayerPrefs.SetString(pref.ToString(), json);
        UnityEngine.PlayerPrefs.Save();
    }

    public static bool LoadPreference(Pref pref, bool defaultValue, int server = 0)
    {
        return LoadPreference(pref, defaultValue ? 1 : 0, server) > 0;
    }

    public static void SavePreference(Pref pref, bool value, int server = 0)
    {
        SavePreference(pref, value ? 1 : 0, server);
    }

    public static int LoadPreference(Pref pref, int defaultValue, int server = 0)
    {
        int returnInt = UnityEngine.PlayerPrefs.GetInt((server > 1 ? server.ToString() : string.Empty) + pref.ToString(), defaultValue);
        // if (pref != Pref.FPS)
        //     Debug.Log("Preference Loaded " + returnInt);
        return returnInt;
    }

    public static void SavePreference(Pref pref, int value, int server = 0)
    {
        UnityEngine.PlayerPrefs.SetInt((server > 1 ? server.ToString() : string.Empty) + pref.ToString(), value);
        UnityEngine.PlayerPrefs.Save();
        // Debug.Log("Preference Saved " + value);
    }

    public static string LoadPreference(Pref pref, string defaultValue, int server = 0)
    {
        string returnStr = UnityEngine.PlayerPrefs.GetString((server > 1 ? server.ToString() : string.Empty) + pref.ToString(), defaultValue);
        // Debug.Log("Preference Loaded " + returnStr);
        return returnStr;
    }

    public static void SavePreference(Pref pref, string value, int server = 0)
    {
        UnityEngine.PlayerPrefs.SetString((server > 1 ? server.ToString() : string.Empty) + pref.ToString(), value);
        UnityEngine.PlayerPrefs.Save();
        // Debug.Log("Preference Saved " + value);
    }

    public static float LoadPreference(Pref pref, float defaultValue, int server = 0)
    {
        float returnFloat = UnityEngine.PlayerPrefs.GetFloat((server > 1 ? server.ToString() : string.Empty) + pref.ToString(), defaultValue);
        // Debug.Log("Preference Loaded " + returnFloat);
        return returnFloat;
    }

    public static void SavePreference(Pref pref, float value, int server = 0)
    {
        UnityEngine.PlayerPrefs.SetFloat((server > 1 ? server.ToString() : string.Empty) + pref.ToString(), value);
        UnityEngine.PlayerPrefs.Save();
        // Debug.Log("Preference Saved " + value);
    }

    public static void Clear()
    {
        UnityEngine.PlayerPrefs.DeleteAll();
        UnityEngine.PlayerPrefs.Save();
    }

    [System.Serializable]
    private class Wrapper<T>
    {
        public List<T> Items;
    }
}
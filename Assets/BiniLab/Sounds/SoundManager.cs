using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Audio;

public enum SoundBGM
{
    NONE = -1,

    bgm_splash = 0,
    bgm_main,
    bgm_ingame,
    bgm_clear,
    bgm_defeat,
    bgm_boss,
    bgm_intro,
}

public enum SoundFX
{
    NONE = -1,

    sfx_click = 0,
    sfx_cancel,
    sfx_popup,
    sfx_login,
    sfx_kitty_survival,
    sfx_tab,
    sfx_dieMonster,
    sfx_attack_bat,
    sfx_item_equip,
    sfx_mergeResultPop,
    sfx_itemLevelUp,
    sfx_growthItemActivate,
    sfx_enemy_hit,
    sfx_levelupPop,
    sfx_rewardsPop,
    sfx_ingameClearPop,
    sfx_ingameDefeatPop,
    sfx_InGameSkillSelectPop,
    
    sfx_shield,
    sfx_guardian,
    sfx_warning,
    sfx_boss_die,

    // 이하 ObjectPoolManager에서 AudioController.Play로 재생하고 있는 사운드들
    aim_missile,
    exp_fire_explosion,
    exp_fire_plain,
    exp_missile,
    prj_ball,
    prj_boomerang,
    prj_godsword,
    prj_lasergun2,
    prj_lasergun,
    prj_missile,
    prj_molotov_cocktail,
    prj_reflect_bullet,
    prj_rock,
    prj_rocket,
    prj_shotgun,
    prj_spacegun,
    prj_storm,
    prj_sword,
    prj_throwing_knife,
    prj_thunderbolt,
}

public enum SoundAMB // 환경음
{
    NONE = -1,

}

public enum SoundVOX // 음성
{
    NONE = -1,

}


public class SoundManager : GameObjectSingleton<SoundManager>
{

    /////////////////////////////////////////////////////////////
    // public

    public bool IsReady => this.isReady;

    [SerializeField] private AudioMixer _mixer;

    public ClockStone.AudioObject PlayBGM(SoundBGM bgm)
    {
        return this.PlayBGM(bgm.ToString());
    }

    public ClockStone.AudioObject PlaySFX(SoundFX sfx, bool forceInSilence = false)
    {
        if (forceInSilence)
            return this.PlaySFXWithoutSilence(sfx.ToString());
        else
            return this.PlaySFX(sfx.ToString());
    }

    public ClockStone.AudioObject PlayAMB(SoundAMB amb)
    {
        return this.PlayAMB(amb.ToString());
    }

    //public ClockStone.AudioObject PlayVOX(SoundVOX vox, bool forceInSilence = false)
    //{
    //    if (forceInSilence)
    //        return this.PlaySFXWithoutSilence(vox.ToString());
    //    else
    //        return this.PlaySFX(vox.ToString());
    //}

    //public ClockStone.AudioObject PlayVOX(string voxString, bool forceInSilence = false)
    //{
    //    if (forceInSilence)
    //        return this.PlaySFXWithoutSilence(voxString);
    //    else
    //        return this.PlaySFX(voxString);
    //}

    public bool StopSFX(SoundFX sfx)
    {
        return this.StopSFX(sfx.ToString());
    }


    public bool StopBGM()
    {
        return AudioController.StopMusic();
    }

    public bool StopBGM(float fadeOut)
    {
        return AudioController.StopMusic(fadeOut);
    }

    public bool StopAMB()
    {
        return AudioController.StopAmbienceSound();
    }

    //public bool StopVOX(string audioID)
    //{
    //    if (!this.isReady) return false;
    //
    //    return AudioController.Stop(audioID);
    //}

    public void Silence(bool isSilence)
    {
        this.isSilence = isSilence;
    }

    public void PauseSFX()
    {
        this.onSFX = false;
    }
    public void UnPauseSFX()
    {
        this.onSFX = Preference.LoadPreference(Pref.SFX_V, true);
    }

    public void PauseBGM()
    {
        if (Preference.LoadPreference(Pref.BGM_V, 0.8f) > 0f)
        {
            int volume = Convert.ToInt32(-80f + 0.01f * 80f);
            _mixer.SetFloat("BGM", volume);
        }
    }

    public void UnPauseBGM()
    {
        if (this.isReady)
            _mixer.SetFloat("BGM", Convert.ToInt32(-80f + Preference.LoadPreference(Pref.BGM_V, 0.8f) * 80f));
        //AudioController.SetCategoryVolume("BGM", Preference.LoadPreference(Pref.BGM_V, 0.8f));
    }

    //public void PauseVOX()
    //{
    //    if (Preference.LoadPreference(Pref.VOX_V, 0.8f) > 0f)
    //    {
    //        int volume = Convert.ToInt32(-80f + 0.01f * 80f);
    //        _mixer.SetFloat("VOX", volume);
    //    }
    //}

    //public void UnPauseVOX()
    //{
    //    if (this.isReady)
    //        _mixer.SetFloat("VOX", Convert.ToInt32(-80f + Preference.LoadPreference(Pref.VOX_V, 0.8f) * 80f));
    //    //AudioController.SetCategoryVolume("BGM", Preference.LoadPreference(Pref.BGM_V, 0.8f));
    //}

    //public void PauseVOXUI()
    //{
    //    if (Preference.LoadPreference(Pref.VOX_V, 0.8f) > 0f)
    //   {
    //        int volume = Convert.ToInt32(-80f + 0.01f * 80f);
    //        _mixer.SetFloat("VOX_UI", volume);
    //    }
    //}

    //public void UnPauseVOXUI()
    //{
    //    if (this.isReady)
    //        _mixer.SetFloat("VOX_UI", Convert.ToInt32(-80f + Preference.LoadPreference(Pref.VOX_V, 0.8f) * 80f));
    //    //AudioController.SetCategoryVolume("BGM", Preference.LoadPreference(Pref.BGM_V, 0.8f));
    //}

    public void OnHighpassFilter() // BGM, AMB, SFX 필터 켜기
    {
        _mixer.SetFloat("Highpass", 2500);
    }

    public void OffHighpassFilter() // BGM, AMB, SFX 필터 끄기
    {
        _mixer.SetFloat("Highpass", 10);
    }

    public void SetBGMVolume(float v)
    {
        int volume = Convert.ToInt32((-80f + v * 80f) * 0.5f);
        if (this.isReady)
            _mixer.SetFloat("BGM", volume);
        if (v == 0)
            _mixer.SetFloat("BGM", -80f);
    }

    public void SetSFXVolume(float v)
    {
        int volume = Convert.ToInt32((-80f + v * 80f) * 0.5f);
        if (this.isReady)
        {
            _mixer.SetFloat("SFX", volume);
            _mixer.SetFloat("AMB", volume);
        }

        if (v == 0)
        {
            _mixer.SetFloat("SFX", -80f);
            _mixer.SetFloat("AMB", -80f);
        }
    }

    //public void SetVOXVolume(float v)
    //{
    //    int volume = Convert.ToInt32((-80f + v * 80f) * 0.5f);
    //    if (this.isReady)
    //        _mixer.SetFloat("VOX", volume);
    //    if (v == 0)
    //        _mixer.SetFloat("VOX", -80f);
    //}

    //public void SetVOXUIVolume(float v)
    //{
    //    int volume = Convert.ToInt32((-80f + v * 80f) * 0.5f);
    //    if (this.isReady)
    //        _mixer.SetFloat("VOX_UI", volume);
    //    if (v == 0)
    //        _mixer.SetFloat("VOX_UI", -80f);
    //}

    public void SetAMBVolume(float v)
    {
        int volume = Convert.ToInt32((-80f + v * 80f) * 0.5f);
        if (this.isReady)
            _mixer.SetFloat("AMB", volume);
        if (v == 0)
            _mixer.SetFloat("AMB", -80f);
    }

    protected override void Awake()
    {
        base.Awake();
        this.UpdateOption();
    }

    /////////////////////////////////////////////////////////////
    // Common Use

    public void PlayButtonClick()
    {
        this.PlaySFX(SoundFX.sfx_click);
    }

    public void PlayCancel()
    {
        this.PlaySFX(SoundFX.sfx_cancel);
    }

    public void UpdateOption()
    {
        this.onBGM = Preference.LoadPreference(Pref.BGM_V, true);
        this.onSFX = Preference.LoadPreference(Pref.SFX_V, true);
    }

    protected void Start()
    {
        // Run.Wait(() => { return AssetBundleManager.Instance.ReadyToStart; }, () =>
        // {
        //     AssetBundleManager.Instance.LoadAsset(AssetBundleType.SOUND, (result) =>
        //     {
        //         if (result != null)
        //         {
        //             GameObject soundManagerPrefab = AssetBundleManager.Instance.LoadObjectFromBundle(AssetBundleType.SOUND, "AudioController");
        //             GameObject bossMonsterObj = Instantiate(soundManagerPrefab, Vector3.zero, Quaternion.identity);
        //             bossMonsterObj.transform.SetParent(this.transform);
        //             this.isReady = true;
        //         }
        //     });
        // });\
        this.isReady = true;
    }

    /////////////////////////////////////////////////////////////
    // private

    private bool isSilence = false;

    private bool onBGM = true;
    private bool onSFX = true;
    private bool onAMB = true;

    private bool isReady = false;

    public ClockStone.AudioObject PlayBGM(string audioID)
    {
        if (!this.isReady) return null;

        if (!this.onBGM)
            return null;

        ClockStone.AudioObject currentAudioObj = AudioController.GetCurrentMusic();
        if (currentAudioObj != null && currentAudioObj.audioID.Equals(audioID))
            return null;

        AudioController.StopMusic();

        return AudioController.PlayMusic(audioID);
    }

    private ClockStone.AudioObject PlaySFX(string audioID)
    {
        if (!this.isReady) return null;

        if (!this.onSFX)
            return null;

        if (this.isSilence)
            return AudioController.Play(audioID, 0.2f);
        else
            return AudioController.Play(audioID);
    }

    public ClockStone.AudioObject PlayAMB(string audioID)
    {
        if (!this.isReady) return null;

        if (!this.onAMB)
            return null;

        ClockStone.AudioObject currentAudioObj = AudioController.GetCurrentAmbienceSound();
        if (currentAudioObj != null && currentAudioObj.audioID.Equals(audioID))
            return null;

        AudioController.StopAmbienceSound();

        return AudioController.PlayAmbienceSound(audioID);
    }

    private ClockStone.AudioObject PlaySFXWithoutSilence(string audioID)
    {
        if (!this.isReady) return null;

        if (!this.onSFX)
            return null;

        Debug.Log("PlaySFXWithoutSilence " + audioID);
        return AudioController.Play(audioID);
    }

    private bool StopSFX(string audioID)
    {
        if (!this.isReady) return false;

        return AudioController.Stop(audioID);
    }
}
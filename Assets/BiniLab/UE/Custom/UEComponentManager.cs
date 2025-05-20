using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UEComponentManager : MonoBehaviour
{
    //singleton
    public static bool Loaded =>_inst != null && _inst.Valid;
    public static UEComponentManager Instance => _inst;
    private static UEComponentManager _inst = null;
    
    private bool AllowMultiInstance => false;
    private bool Valid =>_inst != null;
    
    private List<UEComponent> _uiComponents = new List<UEComponent>();
    
    public void Bind(UEComponent comp)
    {
        _uiComponents.Add(comp);
    }

    public void RefreshUI()
    {
        for (var i = 0; i < _uiComponents.Count; i++)
        {
            _uiComponents[i].RefreshUI();
        }
    }
    
    public void Remove(UEComponent uiComponent)
    {
        _uiComponents.Remove(uiComponent);
    }

    public void OnGlobalButtonClickNotificationEvent(UEButton button)
    {
        if (button.SoundType == UEButtonSoundType.Default)
        {
            SoundManager.Instance.PlayButtonClick();
        }
    }
    
    public void OnGlobalTabClickNotificationEvent(UETabButton tabButton)
    {
        if (tabButton.SoundType == UETabSoundType.Default)
        {
            SoundManager.Instance.PlayButtonClick();
        }
    }
    
    protected void Awake()
    {
        if (_inst != null)
        {
            if (!this.AllowMultiInstance)
                Debug.LogError("UEComponentManager is already attached");

            return;
        }

        _inst = this;

        UEButton.GlobalClickNotificationEvent += OnGlobalButtonClickNotificationEvent;
        UETabButton.GlobalTabNotificationEvent += OnGlobalTabClickNotificationEvent;
    }

    protected void OnDestroy()
    {
        UEButton.GlobalClickNotificationEvent -= OnGlobalButtonClickNotificationEvent;
        UETabButton.GlobalTabNotificationEvent -= OnGlobalTabClickNotificationEvent;
    }
}

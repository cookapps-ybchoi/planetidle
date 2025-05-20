using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class UEComponent : MonoBehaviour
{
    private bool _isDirty = true;

    protected virtual void Awake()
    {
        if (UEComponentManager.Instance != null)
        {
            UEComponentManager.Instance.Bind(this);
        }
    }

    protected virtual void OnDestroy()
    {
        if (UEComponentManager.Instance != null)
        {
            UEComponentManager.Instance.Remove(this);
        }
    }

    protected virtual void OnEnable()
    {
        if (_isDirty)
        {
            _isDirty = false;
            InternalRefresh();
        }
    }

    protected virtual void Update()
    {
        if (_isDirty)
        {
            _isDirty = false;
            InternalRefresh();
        }
    }

    public void RefreshUI()
    {
        _isDirty = true;
    }

    protected virtual void InternalRefresh()
    {
    }
}
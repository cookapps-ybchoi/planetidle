using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenTouch : MonoBehaviour
{
    [SerializeField] private ParticleSystem _fxPrefab;
    [SerializeField] private Camera _touchCamera;

    private List<ParticleSystem> _list = new List<ParticleSystem>();

    private float _width;
    private float _height;

    void Awake()
    {
        _width = (float) Screen.width / 2.0f;
        _height = (float) Screen.height / 2.0f;
    }
    

    private void Update()
    {
        if (Input.touchCount > 0)
        {
            Touch touch = Input.GetTouch(0);

            if (touch.phase == TouchPhase.Began)
            {
                Vector2 pos = touch.position;
                Vector3 fxPos = _touchCamera.ScreenToWorldPoint(touch.position);
                fxPos.z = 0;
                ShowFx(fxPos);
            }
        }
    }

    private void ShowFx(Vector3 pos)
    {
        ParticleSystem ps = _list.Find((c => !c.gameObject.activeSelf));
        if (ps == null)
        {
            ps = Instantiate(_fxPrefab, transform);
            _list.Add(ps);
        }
        ps.transform.position = pos;
        ps.gameObject.SetActive(true);
    }
}
using UnityEngine;
using DG.Tweening;
using System;
public partial class InGameManager
{
    private Camera _mainCamera;

    private void InitCamera()
    {
        _mainCamera = Camera.main;
        _mainCamera.transform.position = new Vector3(0, -10, -10);
    }

    // 인게임 시작시 카메라 연출
    // 인게임 시작은 Y 값 아래로 배치 후 
    // 아래에서 위로 올라오는 연출
    private void StartGameCamera(Action onComplete)
    {
        _mainCamera.transform.DOMoveY(0, 1f).SetEase(Ease.OutQuad).onComplete = () =>
        {
            onComplete?.Invoke();
        };
    }
}

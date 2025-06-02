using UnityEngine;
using DG.Tweening;
using System;
public partial class InGameManager
{
    private const float CAMERA_SIZE_START = 50;
    private const float CAMERA_SIZE_END = 5;
    private Camera _mainCamera;

    private void InitCamera()
    {
        _mainCamera = Camera.main;
        _mainCamera.orthographicSize = CAMERA_SIZE_START;
    }

    // 인게임 시작시 카메라 연출
    // 인게임 시작은 Y 값 아래로 배치 후 
    // 아래에서 위로 올라오는 연출
    private void StartGameCamera(Action onComplete)
    {
        DOTween.To(() => _mainCamera.orthographicSize, x => _mainCamera.orthographicSize = x, CAMERA_SIZE_END, 1f).SetEase(Ease.OutSine).onComplete = () =>
        {
            onComplete?.Invoke();
        };
    }
}

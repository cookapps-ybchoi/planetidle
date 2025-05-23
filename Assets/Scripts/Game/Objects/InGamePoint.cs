using TMPro;
using UnityEngine;
using DG.Tweening;
using Game.ObjectPool;

public class InGamePoint : PoolableObject
{
    private const float MOVE_DURATION = 0.5f;
    private const float MOVE_Y = 0.4f;

    [SerializeField] private TextMeshPro _pointText;

    public override void OnSpawn()
    {
        base.OnSpawn();
        _pointText.alpha = 1f;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    //포인트는 달러로 표시
    public void SetPoint(int point)
    {
        _pointText.text = $"${point}";
        Show();
    }

    /// <summary>
    /// 조금 위로 올라가며 사라짐
    /// </summary>
    public void Show()
    {
        // 위로 올라가는 움직임 (처음엔 빠르게, 나중엔 천천히)
        // 50% 올라갔을때 투명해지기 시작
        // 100% 올라갔을때 사라짐
        Sequence sequence = DOTween.Sequence();

        // 이동 애니메이션
        sequence.Append(transform.DOMoveY(transform.position.y + MOVE_Y, MOVE_DURATION).SetEase(Ease.OutQuad));

        // 투명도 애니메이션 (50% 지점에서 시작)
        sequence.Insert(MOVE_DURATION * 0.5f, _pointText.DOFade(0f, MOVE_DURATION * 0.5f));

        // 완료 후 풀로 반환
        sequence.OnComplete(() =>
        {
            AddressableManager.Instance.ReturnToPool(this);
        });
    }
}

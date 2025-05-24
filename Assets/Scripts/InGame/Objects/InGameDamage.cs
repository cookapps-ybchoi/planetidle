using TMPro;
using UnityEngine;
using DG.Tweening;
using Game.ObjectPool;

public class InGameDamage : PoolableObject
{
    private const float MOVE_DURATION = 0.3f;
    private const float MOVE_Y = 0.2f;

    [SerializeField] private TextMeshPro _damageText;

    public override void OnSpawn()
    {
        base.OnSpawn();
        _damageText.alpha = 1f;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    public void SetDamage(double damage)
    {
        _damageText.text = ((int)damage).ToString();
        Show();
    }

    /// <summary>
    /// 조금 위로 올라가며 사라짐
    /// </summary>
    public void Show()
    {
        // 위로 올라가는 움직임 (처음엔 빠르게, 나중엔 천천히)
        // 위로 다 올라가면 투명해지기 시작
        Sequence sequence = DOTween.Sequence();
        
        // 이동 애니메이션
        sequence.Append(transform.DOMoveY(transform.position.y + MOVE_Y, MOVE_DURATION).SetEase(Ease.OutQuad));
        
        // 투명도 애니메이션 (이동이 완료된 후 시작)
        sequence.Append(_damageText.DOFade(0f, MOVE_DURATION * 0.5f));
        
        // 완료 후 풀로 반환
        sequence.OnComplete(() =>
        {
            AddressableManager.Instance.ReturnToPool(this);
        });
    }
}

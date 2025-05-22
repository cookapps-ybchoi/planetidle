using TMPro;
using UnityEngine;
using DG.Tweening;
using Game.ObjectPool;

public class InGameDamage : PoolableObject
{
    private const float MOVE_DURATION = 0.5f;
    private const float MOVE_Y = 0.3f;

    [SerializeField] private TextMeshPro _damageText;

    public override void OnSpawn()
    {
        base.OnSpawn();
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
        transform.DOMoveY(transform.position.y + MOVE_Y, MOVE_DURATION).SetEase(Ease.OutQuad).OnComplete(() =>
        {
            AddressableManager.Instance.ReturnToPool(this);
        });
    }
}

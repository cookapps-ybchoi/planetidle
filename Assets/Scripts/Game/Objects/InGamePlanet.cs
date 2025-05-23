using UnityEngine;
using System.Collections;
using Game.ObjectPool;
using System.Threading.Tasks;

public class InGamePlanet : PoolableObject
{
    private const float RANGE_THICKNESS_DEFAULT = 0.03f;

    [SerializeField] private SpriteRenderer _planetSprite;
    [SerializeField] private SpriteRenderer _rangeSprite;
    [SerializeField] private int _explosionId = 2;

    private PlanetData _planetData;

    private InGameEnemy _targetEnemy;
    private bool _canAttack = true;
    private bool _isPlayingHitEffect = false;
    private double _hp;
    private double _cachedRange;
    private double _cachedAttackSpeed;
    private double _attackCooldownTime;
    private Coroutine _attackRoutine;

    public override void OnSpawn()
    {
        base.OnSpawn();
        // 행성 데이터
        _planetData = DataManager.Instance.PlanetData;
        _hp = _planetData.GetStatValue(PlanetStatType.Hp);
        _planetSprite.color = Color.white;

        // 캐시된 값 초기화
        _cachedRange = _planetData.GetStatValue(PlanetStatType.Range);
        _cachedAttackSpeed = _planetData.GetStatValue(PlanetStatType.AttackSpeed);
        _attackCooldownTime = _planetData.GetStatValue(PlanetStatType.AttackCooltime) / _cachedAttackSpeed;

        // 초기화 시 범위 표시 업데이트
        DrawRange();

        // 공격 루틴 시작
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
        }
        _attackRoutine = StartCoroutine(AttackRoutine());
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        if (_attackRoutine != null)
        {
            StopCoroutine(_attackRoutine);
            _attackRoutine = null;
        }
    }

    public void TakeDamage(double damage)
    {
        _hp -= damage;
        if (_hp <= 0)
        {
            InGameManager.Instance.GameOver();
        }
        else
        {
            if (!_isPlayingHitEffect)
            {
                _isPlayingHitEffect = true;
                StartCoroutine(PlayHitEffectCoroutine(_planetSprite));
            }
        }

        Debug.Log($"Planet HP: {_hp}");
    }

    public void Finish()
    {
        StartCoroutine(FinishCoroutine());
    }

    private IEnumerator AttackRoutine()
    {
        while (true)
        {
            if (_canAttack)
            {
                CheckAndAttackEnemies();
            }
            yield return null; // 한 프레임 대기
        }
    }

    private void CheckAndAttackEnemies()
    {
        if (_targetEnemy != null && _targetEnemy.IsAlive())
        {
            StartCoroutine(AttackWithDelay(_targetEnemy));
        }
        else
        {
            _targetEnemy = InGameWaveManager.Instance.GetTargetEnemy(transform.position, _cachedRange);
            if (_targetEnemy != null)
            {
                _targetEnemy.OnEnemyDestroyed += OnTargetEnemyDestroyed;
                StartCoroutine(AttackWithDelay(_targetEnemy));
            }
        }
    }

    private void OnTargetEnemyDestroyed(InGameEnemy enemy)
    {
        enemy.OnEnemyDestroyed -= OnTargetEnemyDestroyed;
        if (_targetEnemy == enemy)
        {
            _targetEnemy = null;
        }
    }

    private IEnumerator AttackWithDelay(InGameEnemy enemy)
    {
        _canAttack = false;
        yield return StartCoroutine(Attack(enemy));
        yield return new WaitForSeconds((float)_attackCooldownTime);
        _canAttack = true;
    }

    private void DrawRange()
    {
        double range = _cachedRange * 2f;
        _rangeSprite.transform.localScale = new Vector3((float)range, (float)range, 1);

        //기본 두께는 0.03 기준 range 1. range 증가 값에 역비례
        _rangeSprite.material.SetFloat("_Thickness", RANGE_THICKNESS_DEFAULT / (float)range);
    }

    private IEnumerator PlayHitEffectCoroutine(SpriteRenderer spriteRenderer)
    {
        _isPlayingHitEffect = true;

        Color originalColor = _planetSprite.color;
        Color hitColor = new Color(1f, 0.5f, 0.5f); // 연한 빨간색
        float duration = 0.025f; // 전체 이펙트 지속시간

        // 빨간색으로 변경
        spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(duration);

        // 원래 색으로 복원
        spriteRenderer.color = originalColor;
        _isPlayingHitEffect = false;
    }

    //InGameBullet 을 생성하여 적에게 공격
    private IEnumerator Attack(InGameEnemy enemy)
    {
        var bulletTask = AddressableManager.Instance.GetBullet(1, transform.position, transform.parent);
        yield return new WaitUntil(() => bulletTask.IsCompleted);

        if (bulletTask.Result != null)
        {
            bulletTask.Result.SetTarget(enemy);
        }
    }

    private IEnumerator FinishCoroutine()
    {
        var explosionTask = AddressableManager.Instance.GetExplosion(_explosionId, transform.position, transform.parent);
        yield return new WaitUntil(() => explosionTask.IsCompleted);
        AddressableManager.Instance.ReturnToPool(this);
    }
}

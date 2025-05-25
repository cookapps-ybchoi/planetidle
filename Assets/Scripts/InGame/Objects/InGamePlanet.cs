using UnityEngine;
using System.Collections;
using Game.ObjectPool;
using System;

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
    private double _maxHp;
    private double _hpRecovery;
    private double _range;
    private double _attackSpeed;
    private double _attackCooldownTime;
    private Coroutine _gameRoutine;

    public double CurrrentHp => _hp;

    public override void OnSpawn()
    {
        base.OnSpawn();

        // 이벤트 구독
        InGameEventManager.Instance.OnPlanetStateLevelChanged += OnPlanetStateChanged;
        InGameEventManager.Instance.OnEnemyDestroyed += OnEnemyDestroyed;

        // 행성 데이터
        _planetData = DataManager.Instance.PlanetData;
        _planetData.Initialize();

        // 행성 스프라이트 색상 초기화
        _planetSprite.color = Color.white;

        // 캐시된 값 초기화
        ResetValues();

        // 값 초기화
        UpdateValues();

        // 초기화 시 범위 표시 업데이트
        DrawRange();

        // 게임 루틴 시작
        StartGameRoutine();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();

        // 이벤트 구독 해제
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnPlanetStateLevelChanged -= OnPlanetStateChanged;
            InGameEventManager.Instance.OnEnemyDestroyed -= OnEnemyDestroyed;
        }

        if (_gameRoutine != null)
        {
            StopCoroutine(_gameRoutine);
            _gameRoutine = null;
        }
    }

    public void TakeDamage(double damage)
    {
        _hp -= damage;
        InGameEventManager.Instance.InvokePlanetStateValueChanged(PlanetStatType.Hp, _hp);
        Debug.Log($"TakeDamage: {damage}, hp: {_hp}");
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
    }

    public void Finish()
    {
        StartCoroutine(FinishCoroutine());
    }

    private void OnPlanetStateChanged(PlanetStatType statType, int level)
    {
        UpdateValues();
        DrawRange();
    }

    private void OnEnemyDestroyed(int enemySpawnId)
    {
        if (_targetEnemy != null && _targetEnemy.EnemySpawnId == enemySpawnId)
        {
            _targetEnemy = null;
        }
    }

    private void ResetValues()
    {
        _range = 0;
        _attackSpeed = 0;
        _attackCooldownTime = 0;
        _hp = 0;
        _maxHp = 0;
        _hpRecovery = 0;
    }

    private void UpdateValues()
    {
        _range = _planetData.GetStatValue(PlanetStatType.Range);
        _attackSpeed = _planetData.GetStatValue(PlanetStatType.AttackSpeed);
        _attackCooldownTime = _planetData.GetStatValue(PlanetStatType.AttackCooltime) / _attackSpeed;
        _hpRecovery = _planetData.GetStatValue(PlanetStatType.HpRecovery);

        // 최대 체력 증가 만큼만 체력 증가
        double previousMaxHp = _maxHp;
        _maxHp = _planetData.GetStatValue(PlanetStatType.Hp);
        _hp += _maxHp - previousMaxHp;
        InGameEventManager.Instance.InvokePlanetStateValueChanged(PlanetStatType.Hp, _hp);
    }

    private void StartGameRoutine()
    {
        if (_gameRoutine != null)
        {
            StopCoroutine(_gameRoutine);
        }
        _gameRoutine = StartCoroutine(GameRoutine());
    }

    private IEnumerator GameRoutine()
    {
        float hpRecoveryTimer = 0f;

        while (true)
        {
            // 공격 체크
            if (_canAttack)
            {
                CheckAndAttackEnemies();
            }

            // HP 회복 체크
            hpRecoveryTimer += Time.deltaTime;
            if (hpRecoveryTimer >= 1f)
            {
                if (_hp < _maxHp)
                {
                    _hp = Math.Min(_hp + _hpRecovery, _maxHp);
                    InGameEventManager.Instance.InvokePlanetStateValueChanged(PlanetStatType.Hp, _hp);
                }
                hpRecoveryTimer = 0f;
            }

            yield return null;
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
            _targetEnemy = InGameWaveManager.Instance.GetTargetEnemy(transform.position, _range);
            if (_targetEnemy != null)
            {
                StartCoroutine(AttackWithDelay(_targetEnemy));
            }
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
        double range = _range * 2f;
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

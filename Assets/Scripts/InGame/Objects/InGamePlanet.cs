using UnityEngine;
using System.Collections;
using Game.ObjectPool;
using System;
using DG.Tweening;
using System.Collections.Generic;
using UnityEngine.Pool;
using System.Linq;

public class InGamePlanet : PoolableObject
{
    private const float RANGE_THICKNESS_DEFAULT = 0.03f;

    [SerializeField] private SpriteRenderer _planetSprite;
    [SerializeField] private SpriteRenderer _rangeSprite;
    [SerializeField] private int _explosionId = 2;


    private InGameEnemy _targetEnemy;
    private bool _isReady = false;
    private bool _canAttack = true;
    private bool _isPlayingHitEffect = false;
    private Coroutine _gameRoutine;

    public bool IsReady => _isReady;
    public PlanetData PlanetData { get; private set; }

    public override void OnSpawn()
    {
        base.OnSpawn();

        _isReady = false;

        // 이벤트 구독
        InGameEventHandler.OnEnemyDestroyed += OnEnemyDestroyed;
        InGameEventHandler.OnPlanetStateValueChanged += OnPlanetStateValueChanged;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();

        // 이벤트 구독 해제
        InGameEventHandler.OnEnemyDestroyed -= OnEnemyDestroyed;
        InGameEventHandler.OnPlanetStateValueChanged -= OnPlanetStateValueChanged;

        if (_gameRoutine != null)
        {
            StopCoroutine(_gameRoutine);
            _gameRoutine = null;
        }
    }

    public void InitData(PlanetData planetData)
    {
        PlanetData = planetData;
    }

    public void ReadyToStart()
    {
        if (_rangeSprite != null)
        {
            _rangeSprite.transform.localScale = new Vector3(0, 0, 1);
        }
    }

    public void PlayStart()
    {
        // 행성 범위 연출
        PlayShowRange();
    }

    public double GetStateValue(PlanetStatType statType)
    {
        return PlanetData.GetStateValue(statType);
    }

    public void TakeDamage(double damage)
    {
        PlanetData.Hp -= damage;
        InGameEventHandler.InvokePlanetStateValueChanged(PlanetStatType.Hp, PlanetData.Hp);
        if (PlanetData.Hp <= 0)
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

    private void OnPlanetStateValueChanged(PlanetStatType statType, double value)
    {
        if (statType == PlanetStatType.Range)
        {
            UpdateRange();
        }
    }
    private void OnEnemyDestroyed(int enemySpawnId, bool isKilled)
    {
        if (_targetEnemy != null && _targetEnemy.EnemySpawnId == enemySpawnId)
        {
            _targetEnemy = null;
        }
    }

    private void PlayShowRange()
    {
        Color originalColor = _rangeSprite.material.GetColor("_Color");

        //투명도가 0에서 1로 변경되는 애니메이션
        DOTween.To(() => 0f, (float alpha) =>
        {
            _rangeSprite.material.SetColor("_Color", new Color(1f, 1f, 1f, alpha));
        }, 1f, 0.5f).SetEase(Ease.InQuad);


        DOTween.To(() => 0f, (float range) =>
        {
            DrawRange(range);
        }, (float)PlanetData.AttackRange, 1f).SetEase(Ease.OutBack).onComplete = () =>
        {
            _isReady = true;
            StartGameRoutine();
        };
    }

    private void UpdateRange()
    {
        // 행성 범위 업데이트
        _rangeSprite.transform.localScale = new Vector3(0, 0, 1);

        Color originalColor = _rangeSprite.material.GetColor("_Color");
        _rangeSprite.material.SetColor("_Color", Color.green);

        DOTween.To(() => 0f, (float range) =>
        {
            DrawRange(range);
        }, (float)PlanetData.AttackRange, 1f).SetEase(Ease.OutBack).onComplete = () =>
        {
            DOTween.To(() => originalColor, (Color color) =>
            {
                _rangeSprite.material.SetColor("_Color", color);
            }, Color.green, 0.5f).SetEase(Ease.InOutQuad).SetLoops(4, LoopType.Yoyo).onComplete = () =>
            {
                DrawRange(PlanetData.AttackRange);
            };
        };
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
                if (PlanetData.Hp < PlanetData.MaxHp)
                {
                    PlanetData.Hp = Math.Min(PlanetData.Hp + PlanetData.HpRecovery, PlanetData.MaxHp);
                    InGameEventHandler.InvokePlanetStateValueChanged(PlanetStatType.Hp, PlanetData.Hp);
                }
                hpRecoveryTimer = 0f;
            }

            yield return null;
        }
    }

    private void CheckAndAttackEnemies()
    {
        if (_canAttack)
        {
            UpdateTargetsOnRange(transform.position, PlanetData.AttackRange);
            StartCoroutine(AttackTargetsOnRange());
        }
    }

    private void UpdateTargetsOnRange(Vector3 position, double range)
    {

        foreach (var enemy in InGameWaveManager.Instance.Enemies)
        {
            if (enemy == null || !enemy.IsAlive || enemy.IsOnRange) continue;

            Vector3 direction = enemy.transform.position - position;
            float distance = direction.magnitude;
            float actualDistance = distance - enemy.EnemySize;

            enemy.SetOnRange(actualDistance <= range);
        }
    }

    private IEnumerator AttackTargetsOnRange()
    {
        try
        {
            List<InGameEnemy> enemies = ListPool<InGameEnemy>.Get();
            enemies.AddRange(InGameWaveManager.Instance.Enemies.Where(enemy => enemy != null && enemy.IsAlive && enemy.IsOnRange));

            if (enemies != null && enemies.Count > 0)
            {
                _canAttack = false;
                float attackCooltime = (float)PlanetData.AttackCooltime / (float)PlanetData.AttackSpeed;

                int attackCount = Math.Min((int)PlanetData.AttackCount, enemies.Count);

                // 공격 대상이 여러 개일 경우 가장 가까운 대상부터 공격
                // 공격 대상이 공격 가능 갯수 보다 많을 경우에만 거리순으로 소트
                if (enemies.Count > attackCount)
                {
                    enemies.Sort((a, b) =>
                        Vector3.Distance(a.transform.position, transform.position)
                        .CompareTo(Vector3.Distance(b.transform.position, transform.position)));
                }

                for (int i = 0; i < attackCount; i++)
                {
                    StartCoroutine(Attack(enemies[i]));
                }

                yield return new WaitForSeconds(attackCooltime);
            }
        }
        finally
        {
            _canAttack = true;
        }
    }

    private void DrawRange(double range)
    {
        float fullRange = (float)range * 2f;
        _rangeSprite.transform.localScale = new Vector3(fullRange, fullRange, 1);

        //기본 두께는 0.03 기준 range 1. range 증가 값에 역비례
        _rangeSprite.material.SetFloat("_Thickness", RANGE_THICKNESS_DEFAULT / fullRange);
    }

    private IEnumerator PlayHitEffectCoroutine(SpriteRenderer spriteRenderer)
    {
        _isPlayingHitEffect = true;

        Color originalColor = _planetSprite.color;
        Color hitColor = new Color(1f, 0.5f, 0.5f); // 연한 빨간색
        float duration = 0.02f; // 전체 이펙트 지속시간

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

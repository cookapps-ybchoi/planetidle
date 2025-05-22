using UnityEngine;
using System.Collections;
using Game.ObjectPool;

public class InGamePlanet : PoolableObject
{
    private const float RANGE_THICKNESS_DEFAULT = 0.03f;

    [SerializeField] private SpriteRenderer _planetSprite;
    [SerializeField] private SpriteRenderer _rangeSprite;

    private PlanetData _planetData;

    private InGameEnemy _targetEnemy;
    private bool _canAttack = true;

    public override void OnSpawn()
    {
        base.OnSpawn();
        // 행성 데이터
        _planetData = DataManager.Instance.PlanetData;

        // 초기화 시 범위 표시 업데이트
        DrawRange();
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
    }

    private void Update()
    {
        CheckEnemies();
    }

    //사정거리 안에 있는 적 중에 가장 가까운 적을 찾아서 공격
    private void CheckEnemies()
    {
        if (!_canAttack) return;

        if (_targetEnemy != null && _targetEnemy.IsAlive())
        {
            StartCoroutine(AttackWithDelay(_targetEnemy));
        }
        else
        {
            float range = _planetData.GetStatValue(PlanetStatType.Range);

            _targetEnemy = InGameWaveManager.Instance.GetTargetEnemy(transform.position, range);
            if (_targetEnemy != null)
            {
                _targetEnemy.OnEnemyDestroyed += OnTargetEnemyDestroyed;
                StartCoroutine(AttackWithDelay(_targetEnemy));
            }
        }
    }

    private void OnTargetEnemyDestroyed(InGameEnemy enemy)
    {
        _targetEnemy = null;
    }

    private IEnumerator AttackWithDelay(InGameEnemy enemy)
    {
        _canAttack = false;
        Attack(enemy);

        // 공격 속도에 따른 딜레이 계산 (공격 속도가 높을수록 딜레이 감소)
        float attackSpeed = _planetData.GetStatValue(PlanetStatType.AttackSpeed);
        float baseDelay = Constants.PLANET_ATTACK_DELAY_DEFUALT; // 기본 딜레이
        float delay = baseDelay / attackSpeed;

        yield return new WaitForSeconds(delay);
        _canAttack = true;
    }

    private void DrawRange()
    {
        float range = _planetData.GetStatValue(PlanetStatType.Range) * 2f;
        _rangeSprite.transform.localScale = new Vector3(range, range, 1);

        //기본 두께는 0.03 기준 range 1. range 증가 값에 역비례
        _rangeSprite.material.SetFloat("_Thickness", RANGE_THICKNESS_DEFAULT / range);
    }

    //InGameBullet 을 생성하여 적에게 공격
    private async void Attack(InGameEnemy enemy)
    {
        InGameBullet bullet = await AddressableManager.Instance.GetBullet(1, transform.position, transform.parent);
        bullet.SetTarget(enemy);
    }
}

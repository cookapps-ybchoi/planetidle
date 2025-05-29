using UnityEngine;
using Game.ObjectPool;
using System.Collections;
using DG.Tweening;

public enum EnemyState
{
    Idle,
    Moving,
    Attacking,
    Destroy,
    Finish,
}

public class InGameEnemy : PoolableObject
{

    public float EnemySize => _size;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private TrailRenderer[] _trailRenderer;
    [SerializeField] private float _size = 0.25f;
    [SerializeField] private int _explosionId = 1;

    private EnemyData _enemyData;
    private bool _isPlayingHitEffect = false;
    private bool _canAttack = true;

    public EnemyState CurrentState { get; private set; }
    public int EnemySpawnId { get; private set; }
    public bool IsOnRange { get; private set; }

    public bool IsAlive
    {
        get
        {
            if (_enemyData == null) return false;
            else if (CurrentState == EnemyState.Destroy || CurrentState == EnemyState.Finish) return false;
            return _enemyData.MaxHp > 0;
        }
    }

    public override void OnSpawn()
    {
        base.OnSpawn();
        LookAtPlanet();
        _spriteRenderer.color = new Color(1, 1, 1, 0);
        _spriteRenderer.DOFade(1, 0.2f).SetEase(Ease.InQuad);
        foreach (var trail in _trailRenderer)
        {
            trail.Clear();
        }
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        _enemyData = null;
    }

    public void Initialize(EnemyData enemyData, int enemySpawnId)
    {
        _enemyData = enemyData.Copy();
        CurrentState = EnemyState.Moving;
        EnemySpawnId = enemySpawnId;
        IsOnRange = false;

        if (_enemyData.MetaData.EnemyType == EnemyType.Boss)
        {
            InGameEventHandler.InvokeBossStateChanged(_enemyData);
        }
    }

    public void SetOnRange(bool isOnRange)
    {
        IsOnRange = isOnRange;
    }

    // 데미지 처리
    public void TakeDamage(double damage)
    {
        //상태가 Destroy 또는 Finish 일 때 스킵
        if (CurrentState == EnemyState.Destroy || CurrentState == EnemyState.Finish) return;

        _enemyData.ChangeHp(-damage);

        if (_enemyData.MetaData.EnemyType == EnemyType.Boss)
        {
            InGameEventHandler.InvokeBossStateChanged(_enemyData);
        }

        if (_enemyData.Hp <= 0)
        {
            CurrentState = EnemyState.Destroy;

            if (_enemyData.MetaData.EnemyType == EnemyType.Boss)
            {
                InGameEventHandler.InvokeBossWaveCompleted(_enemyData);
            }
        }
        else
        {
            if (!_isPlayingHitEffect)
            {
                StartCoroutine(PlayHitEffectCoroutine(damage));
            }
        }
    }

    public void Finish()
    {
        CurrentState = EnemyState.Finish;
        // 적 처리 완료 이벤트 호출
        InGameEventHandler.InvokeEnemyDestroyed(EnemySpawnId, true);
        StartCoroutine(FinishCoroutine());
    }

    public void Stop()
    {
        CurrentState = EnemyState.Finish;
        // 적 처리 완료 이벤트 호출
        InGameEventHandler.InvokeEnemyDestroyed(EnemySpawnId, false);
        StartCoroutine(StopCoroutine());
    }

    private void Update()
    {
        switch (CurrentState)
        {
            case EnemyState.Moving:
                if (InGameManager.Instance.IsPlaying == false) return;
                else if (Vector3.Distance(transform.position, InGameManager.Instance.Planet.transform.position) <= _enemyData.AttackRange + _size)
                {
                    // 행성에 도달했으면 공격 상태로 전환
                    CurrentState = EnemyState.Attacking;
                }
                else
                {
                    // 행성에 도달하지 않았으면 이동
                    MoveToPlanet();
                }
                break;
            case EnemyState.Attacking:
                if (_canAttack)
                {
                    StartCoroutine(AttackPlanetCoutine());
                }
                break;
            case EnemyState.Destroy:
                Finish();
                break;
        }
    }

    private IEnumerator FinishCoroutine()
    {
        var explosionTask = AddressableManager.Instance.GetExplosion(_explosionId, transform.position, transform.parent);
        yield return new WaitUntil(() => explosionTask.IsCompleted);

        var pointTask = AddressableManager.Instance.GetPoint(transform.position, transform.parent);
        yield return new WaitUntil(() => pointTask.IsCompleted);
        InGamePoint pointObject = pointTask.Result;
        pointObject.SetPoint(_enemyData.Point);
        InGameManager.Instance.AddExp(_enemyData.Point);

        InGameWaveManager.Instance.RemoveEnemy(this);
        AddressableManager.Instance.ReturnToPool(this);
    }

    private IEnumerator StopCoroutine()
    {
        var explosionTask = AddressableManager.Instance.GetExplosion(_explosionId, transform.position, transform.parent);
        yield return new WaitUntil(() => explosionTask.IsCompleted);

        InGameWaveManager.Instance.RemoveEnemy(this);
        AddressableManager.Instance.ReturnToPool(this);
    }

    // 행성을 바라봄
    private void LookAtPlanet()
    {
        if (InGameManager.Instance.Planet == null) return;

        Vector3 direction = InGameManager.Instance.Planet.transform.position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    // 행성으로 이동
    private void MoveToPlanet()
    {
        if (InGameManager.Instance.Planet == null) return;

        transform.position = Vector3.MoveTowards(transform.position, InGameManager.Instance.Planet.transform.position, _enemyData.MoveSpeed * Time.deltaTime);
    }

    // 행성에 공격 딜레이 대기 후 지속 공격
    private IEnumerator AttackPlanetCoutine()
    {
        _canAttack = false;
        yield return new WaitForSeconds(_enemyData.AttackDelay);
        if (IsAlive == false) yield break;
        else if (InGameManager.Instance.Planet == null) yield break;
        InGameManager.Instance.Planet.TakeDamage(_enemyData.AttackPower);
        _canAttack = true;
    }

    // 데미지 효과 재생(빨간색으로 페이드, 데미지 표시)
    private IEnumerator PlayHitEffectCoroutine(double damage)
    {
        var damageTask = AddressableManager.Instance.GetDamage(transform.position, transform.parent);
        yield return new WaitUntil(() => damageTask.IsCompleted);
        InGameDamage damageObject = damageTask.Result;
        damageObject.SetDamage(damage);

        _isPlayingHitEffect = true;

        Color originalColor = _spriteRenderer.color;
        Color hitColor = new Color(1f, 0.5f, 0.5f); // 연한 빨간색
        float duration = 0.05f; // 전체 이펙트 지속시간

        // 빨간색으로 변경
        _spriteRenderer.color = hitColor;
        yield return new WaitForSeconds(duration);

        // 원래 색으로 복원
        _spriteRenderer.color = originalColor;
        _isPlayingHitEffect = false;
    }

    // 적의 크기를 원형으로 표시
    private void OnDrawGizmos()
    {
        //공격대상이 된경우 빨강색
        Gizmos.color = IsOnRange ? Color.red : Color.yellow;
        int segments = 32;
        Vector3 previousSizePoint = transform.position + new Vector3(_size, 0f, 0f);
        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * 360f * Mathf.Deg2Rad;
            Vector3 newSizePoint = transform.position + new Vector3(
                Mathf.Cos(angle) * _size,
                Mathf.Sin(angle) * _size,
                0f
            );
            Gizmos.DrawLine(previousSizePoint, newSizePoint);
            previousSizePoint = newSizePoint;
        }
    }
}

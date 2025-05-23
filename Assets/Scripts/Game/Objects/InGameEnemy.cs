using UnityEngine;
using Game.ObjectPool;
using System;
using System.Threading.Tasks;
using System.Collections;

public class InGameEnemy : PoolableObject
{
    private enum EnemyState
    {
        Idle,
        Moving,
        Attacking,
        Destroy,
        Finish,
    }

    public float EnemySize => _size;
    public event Action<InGameEnemy> OnEnemyDestroyed;

    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private float _size = 0.25f;
    [SerializeField] private int _explosionId = 1;

    private EnemyState _currentState = EnemyState.Idle;
    private EnemyData _enemyData;
    private bool _isPlayingHitEffect = false;
    private bool _canAttack = true;


    public override void OnSpawn()
    {
        base.OnSpawn();
        LookAtPlanet();
        _spriteRenderer.color = Color.white;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        _enemyData = null;
        OnEnemyDestroyed = null;
    }

    public void Initialize(EnemyData enemyData, int waveLevel)
    {
        _enemyData = enemyData.Copy();
        _enemyData.SetLevel(waveLevel);
        _currentState = EnemyState.Moving;
    }

    public bool IsAlive()
    {
        if (_enemyData == null) return false;
        else if (_currentState == EnemyState.Destroy || _currentState == EnemyState.Finish) return false;
        return _enemyData.Hp > 0;
    }

    public void TakeDamage(double damage)
    {
        //상태가 Destroy 또는 Finish 일 때 스킵
        if (_currentState == EnemyState.Destroy || _currentState == EnemyState.Finish) return;

        _enemyData.ChangeHp(-damage);
        if (_enemyData.Hp <= 0)
        {
            _currentState = EnemyState.Destroy;
        }
        else
        {
            if (!_isPlayingHitEffect)
            {
                StartCoroutine(PlayHitEffectCoroutine(_spriteRenderer));
            }
        }

        Debug.Log($"Damage: {damage} Hp: {_enemyData.Hp}");
    }

    public void Finish()
    {
        _currentState = EnemyState.Finish;
        OnEnemyDestroyed?.Invoke(this);
        StartCoroutine(FinishCoroutine());
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Moving:
                if (InGameManager.Instance.IsPlaying == false) return;
                else if (Vector3.Distance(transform.position, InGameManager.Instance.Planet.transform.position) <= _enemyData.AttackRange + _size)
                {
                    // 행성에 도달했으면 공격 상태로 전환
                    _currentState = EnemyState.Attacking;
                }
                else
                {
                    // 행성에 도달하지 않았으면 이동
                    MoveToPlanet();
                }
                break;
        }
    }

    //행성에게 공격 우선권을 주고 공격 처리
    private void LateUpdate()
    {
        switch (_currentState)
        {
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
        yield return new WaitForSeconds(_enemyData.AttackDelay);
        if (IsAlive())
            InGameManager.Instance.Planet.TakeDamage(_enemyData.AttackPower);
        _canAttack = true;
    }

    private IEnumerator PlayHitEffectCoroutine(SpriteRenderer spriteRenderer)
    {
        _isPlayingHitEffect = true;

        Color originalColor = spriteRenderer.color;
        Color hitColor = Color.red;
        float duration = 0.2f; // 전체 이펙트 지속시간
        float halfDuration = duration * 0.5f;

        // 빨간색으로 페이드
        float elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            spriteRenderer.color = Color.Lerp(originalColor, hitColor, t);
            yield return null;
        }

        // 원래 색으로 페이드백
        elapsed = 0f;
        while (elapsed < halfDuration)
        {
            elapsed += Time.deltaTime;
            float t = elapsed / halfDuration;
            spriteRenderer.color = Color.Lerp(hitColor, originalColor, t);
            yield return null;
        }

        spriteRenderer.color = originalColor; // 원래 색으로 확실히 복원
        _isPlayingHitEffect = false;
    }

    // 적의 크기를 원형으로 표시
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.yellow;
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

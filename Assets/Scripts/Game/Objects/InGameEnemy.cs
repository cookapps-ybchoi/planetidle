using UnityEngine;
using Game.ObjectPool;
using System;

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

    [SerializeField] private float _size = 0.25f;

    [SerializeField] private int _explosionId = 1;

    private EnemyState _currentState = EnemyState.Idle;
    private EnemyData _enemyData;


    public override void OnSpawn()
    {
        base.OnSpawn();
        LookAtPlanet();
        _currentState = EnemyState.Moving;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        _enemyData = null;
        OnEnemyDestroyed = null;
    }

    public void SetData(EnemyData enemyData, int waveLevel)
    {
        _enemyData = enemyData.Copy();
        _enemyData.SetLevel(waveLevel);
    }

    public bool IsAlive()
    {
        if (_enemyData == null) return false;
        return _enemyData.Hp > 0;
    }

    public void TakeDamage(double damage)
    {
        _enemyData.ChangeHp(-damage);
        if (_enemyData.Hp <= 0)
        {
            _currentState = EnemyState.Destroy;
        }

        Debug.Log($"Damage: {damage} Hp: {_enemyData.Hp}");
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Moving:
                if (Vector3.Distance(transform.position, InGameManager.Instance.GetPlanetTransform().position) <= _enemyData.AttackRange + _size)
                {
                    Debug.Log("행성에 도달했습니다.");
                    _currentState = EnemyState.Attacking;
                }
                else
                {
                    MoveToPlanet();
                }
                break;

            case EnemyState.Attacking:
                AttackPlanet();
                break;

            case EnemyState.Destroy:
                Finish();
                break;
        }
    }

    private async void Finish()
    {
        _currentState = EnemyState.Finish;

        OnEnemyDestroyed?.Invoke(this);

        await AddressableManager.Instance.GetExplosion(_explosionId, transform.position, transform.parent);
        InGameWaveManager.Instance.RemoveEnemy(this);
        AddressableManager.Instance.ReturnToPool(this);
    }

    // 행성을 바라봄
    private void LookAtPlanet()
    {
        Vector3 direction = InGameManager.Instance.GetPlanetTransform().position - transform.position;
        float angle = Mathf.Atan2(direction.y, direction.x) * Mathf.Rad2Deg - 90f;
        transform.rotation = Quaternion.Euler(0, 0, angle);
    }

    private void MoveToPlanet()
    {
        transform.position = Vector3.MoveTowards(transform.position, InGameManager.Instance.GetPlanetTransform().position, _enemyData.MoveSpeed * Time.deltaTime);
    }

    private void AttackPlanet()
    {
        // 행성에 공격
        // InGameManager.Instance.GetPlanetTransform().GetComponent<InGamePlanet>().TakeDamage(_enemyData.AttackPower);
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

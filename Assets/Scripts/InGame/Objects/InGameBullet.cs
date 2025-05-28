using UnityEngine;
using Game.ObjectPool;

public class InGameBullet : PoolableObject
{
    private enum BulletState
    {
        Idle,
        Moving,
        Destroy,
        Finish,
    }

    private BulletState _currentState = BulletState.Idle;
    private InGameEnemy _target;
    private float _bulletSpeed = Constants.PLANET_BULLET_SPEED;
    private Vector3 _direction;

    public override void OnSpawn()
    {
        base.OnSpawn();
        _target = null;
        _currentState = BulletState.Idle;
        InGameEventManager.Instance.OnEnemyDestroyed += OnTargetDestroyed;
    }

    public override void OnDespawn()
    {
        base.OnDespawn();
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnEnemyDestroyed -= OnTargetDestroyed;
        }
    }

    public void SetTarget(InGameEnemy target)
    {
        _target = target;
        _currentState = BulletState.Moving;
    }

    private void OnTargetDestroyed(int enemySpawnId, bool isKilled)
    {
        if (_target != null && _target.EnemySpawnId == enemySpawnId)
        {
            _target = null;
            _currentState = BulletState.Destroy;
        }
    }

    private void Update()
    {
        switch (_currentState)
        {
            case BulletState.Moving:
                if (_target == null)
                {
                    _currentState = BulletState.Destroy;
                }
                else if (Vector3.Distance(transform.position, _target.transform.position) <= 0.1f)
                {
                    double damage = InGameManager.Instance.Planet.GetStateValue(PlanetStatType.AttackPower);
                    _target.TakeDamage(damage);
                    _currentState = BulletState.Destroy;
                }
                else
                {
                    _direction = _target.transform.position - transform.position;
                    MoveToDirection();
                }
                break;
            case BulletState.Destroy:
                Finish();
                break;
        }
    }

    private void MoveToDirection()
    {
        transform.position += _direction.normalized * (_bulletSpeed * Time.deltaTime);
    }

    private void Finish()
    {
        _currentState = BulletState.Finish;
        AddressableManager.Instance.ReturnToPool(this);
    }
}

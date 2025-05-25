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

    private void OnTargetDestroyed(int enemySpawnId)
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
                    double damage = InGameManager.Instance.GetPlanetStateValue(PlanetStatType.AttackPower);
                    _target.TakeDamage(damage);
                    _currentState = BulletState.Destroy;
                }
                else
                {
                    MoveToTarget();
                }
                break;
            case BulletState.Destroy:
                Finish();
                break;
        }
    }

    private void MoveToTarget()
    {
        transform.position = Vector3.MoveTowards(transform.position, _target.transform.position, Constants.PLANET_BULLET_SPEED * Time.deltaTime);
    }

    private void Finish()
    {
        _currentState = BulletState.Finish;
        AddressableManager.Instance.ReturnToPool(this);
    }
}

using UnityEngine;
using Game.ObjectPool;

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
    }

    public void SetData(EnemyData enemyData, int waveLevel)
    {
        _enemyData = enemyData;
        _enemyData.SetLevel(waveLevel);
    }

    public void TakeDamage(double damage)
    {
        _enemyData.ChangeHp(-damage);
        if (_enemyData.Hp <= 0)
        {
            _currentState = EnemyState.Destroy;
        }
    }

    private void Update()
    {
        switch (_currentState)
        {
            case EnemyState.Moving:
                if (Vector3.Distance(transform.position, InGameManager.Instance.GetPlanetTransform().position) <= _enemyData.AttackRange)
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

        await AddressableManager.Instance.GetExplosion(1, transform.position, transform.parent);
        InGameWaveManager.Instance.Enemies.Remove(this);
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
}

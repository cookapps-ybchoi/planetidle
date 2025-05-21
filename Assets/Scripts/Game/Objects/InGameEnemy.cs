using UnityEngine;

public class InGameEnemy : MonoBehaviour, InGameObject
{
    private enum EnemyState
    {
        Idle,
        Moving,
        Attacking
    }

    private EnemyState _currentState = EnemyState.Idle;
    private EnemyData _enemyData;

    public void Initialize()
    {
        LookAtPlanet();
        _currentState = EnemyState.Moving;
    }

    public void SetData(EnemyData enemyData)
    {
        _enemyData = enemyData;
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
        }
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

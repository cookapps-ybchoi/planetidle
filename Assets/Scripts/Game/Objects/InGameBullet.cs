using UnityEngine;

public class InGameBullet : MonoBehaviour, InGameObject
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

    public void Initialize()
    {
        _target = null;
        _currentState = BulletState.Idle;
    }

    public void SetTarget(InGameEnemy target)
    {
        _target = target;
        _currentState = BulletState.Moving;
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
                else if(Vector3.Distance(transform.position, _target.transform.position) <= 0.1f)
                {
                    _target.TakeDamage(InGameManager.Instance.GetPlanetAttackPower());
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
        
        ObjectPoolManager.Instance.ReturnToPool(gameObject.name, gameObject);
    }
}

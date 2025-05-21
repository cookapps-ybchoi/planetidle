using UnityEngine;

public class InGameEnemy : MonoBehaviour, InGameObject
{
    private float _moveSpeed = 1f;
    private bool _isMoving = false;
    public void Initialize()
    {
        LookAtPlanet();
        _isMoving = true;
    }

    private void Update()
    {
        if (_isMoving)
        {
            MoveToPlanet();
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
        transform.position = Vector3.MoveTowards(transform.position, InGameManager.Instance.GetPlanetTransform().position, _moveSpeed * Time.deltaTime);
    }
    
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Planet"))
        {
            Debug.Log("행성에 도달했습니다.");
        }
    }

}

using UnityEngine;
using System.Threading.Tasks;
using System.Collections;

public class InGamePlanet : MonoBehaviour, InGameObject
{
    [SerializeField] private SpriteRenderer _planetSprite;
    [SerializeField] private SpriteRenderer _rangeSprite;
    private PlanetData _planetData;
    private bool _canAttack = true;

    public void Initialize()
    {
        // 행성 데이터
        _planetData = DataManager.Instance.PlanetData;

        // 초기화 시 범위 표시 업데이트
        DrawRange();
    }

    private void Update()
    {
        CheckEnemies();
    }

    //사정거리 안에 있는 적 중에 가장 가까운 적을 찾아서 공격
    private void CheckEnemies()
    {
        if (!_canAttack) return;

        float range = _planetData.GetStatValue(PlanetStatType.Range);
        float rangeSquared = range * range;

        InGameEnemy closestEnemy = null;
        float closestDistanceSquared = float.MaxValue;

        Debug.Log($"전체 적 수: {InGameWaveManager.Instance.Enemies.Count}");
        Debug.Log($"현재 행성 범위: {range}");

        foreach (var enemy in InGameWaveManager.Instance.Enemies)
        {
            if (enemy == null) continue;

            Vector3 direction = enemy.transform.position - transform.position;
            float distanceSquared = direction.sqrMagnitude;

            if (distanceSquared <= rangeSquared && distanceSquared < closestDistanceSquared)
            {
                closestEnemy = enemy;
                closestDistanceSquared = distanceSquared;
            }
        }

        if (closestEnemy != null)
        {
            float actualDistance = Mathf.Sqrt(closestDistanceSquared);
            Debug.Log($"공격 대상 : {closestEnemy.name}, 거리: {actualDistance:F2}, 범위: {range:F2}");
            StartCoroutine(AttackWithDelay(closestEnemy));
        }
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

        //기본 두께는 0.05 기준 range 1. range 증가 값에 역비례
        _rangeSprite.material.SetFloat("_Thickness", 0.05f / range);
    }

    //InGameBullet 을 생성하여 적에게 공격
    private async void Attack(InGameEnemy enemy)
    {
        InGameBullet bullet = await AddressableManager.Instance.GetBullet(1, transform.position, transform);
        bullet.Initialize();
        bullet.SetTarget(enemy);
    }
}

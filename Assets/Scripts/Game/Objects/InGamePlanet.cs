using UnityEngine;

public class InGamePlanet : MonoBehaviour, InGameObject
{
    [SerializeField] private int segments = 36;
    [SerializeField] private Color rangeColor = new Color(1f, 0f, 0f, 0.5f);
    [SerializeField] private LineRenderer rangeIndicator;
    
    private PlanetData _planetData;

    // Update is called once per frame
    private void Update()
    {
        //제자리에서 회전
        transform.Rotate(Vector3.back, Time.deltaTime * 10f);
    }

    public void Initialize()
    {
        // 행성 데이터
        _planetData = DataManager.Instance.PlanetData;

        // 초기화 시 범위 표시 업데이트
        DrawRangeCircle();
    }

    private void DrawRangeCircle()
    {
        rangeIndicator.useWorldSpace = false;
        rangeIndicator.startWidth = 0.02f;
        rangeIndicator.endWidth = 0.02f;
        rangeIndicator.startColor = rangeColor;
        rangeIndicator.endColor = rangeColor;
        rangeIndicator.sortingOrder = 1; // 2D 환경에서의 렌더링 순서 설정
        rangeIndicator.positionCount = segments + 1;

        float angleStep = 360f / segments;
        float range = _planetData.GetStatValue(PlanetStatType.Range);
        
        for (int i = 0; i <= segments; i++)
        {
            float angle = i * angleStep * Mathf.Deg2Rad;
            float x = Mathf.Cos(angle) * range;
            float y = Mathf.Sin(angle) * range;
            rangeIndicator.SetPosition(i, new Vector3(x, y, 0));
        }
    }

    // 공격 범위 설정
    public void SetAttackRange()
    {
        _planetData.IncreaseLevel(PlanetStatType.Range);
        DrawRangeCircle();
    }
}

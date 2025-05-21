using UnityEngine;

public class InGamePlanet : MonoBehaviour, InGameObject
{
    [SerializeField] private SpriteRenderer _planetSprite;
    [SerializeField] private SpriteRenderer _rangeSprite;
    private PlanetData _planetData;

    public void Initialize()
    {
        // 행성 데이터
        _planetData = DataManager.Instance.PlanetData;

        // 초기화 시 범위 표시 업데이트
        DrawRange();
    }

    private void DrawRange()
    {
        float range = _planetData.GetStatValue(PlanetStatType.Range);
        _rangeSprite.transform.localScale = new Vector3(range, range, 1);

        //기본 두께는 0.05 기준 range 1. range 증가 값에 역비례
        _rangeSprite.material.SetFloat("_Thickness", 0.05f / range);
    }
}

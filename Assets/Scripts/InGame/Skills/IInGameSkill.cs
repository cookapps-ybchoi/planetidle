using UnityEngine;

public interface IInGameSkill
{
    InGameSkillId Id { get; }
    InGameSkillType Type { get; }
    int Level { get; }
    void Apply(InGamePlanet planet);         // 스킬 효과 적용
    void LevelUp();                    // 레벨 상승
    bool IsMaxLevel { get; }
}

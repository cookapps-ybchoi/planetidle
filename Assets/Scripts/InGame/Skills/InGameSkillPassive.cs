using UnityEngine;

public class InGameSkillAttackPower : InGameBaseSkill
{
    public InGameSkillAttackPower() { }

    public override void Apply(InGamePlanet planet)
    {
        // 스킬 효과: 공격력 증가
        planet.PlanetData.SetStateValue(PlanetStatType.AttackPower, planet.PlanetData.MetaData.AttackPower * (1 + MetaData.FirstValue * Level));

        // 스페셜 효과: 관통 혹은 각도 확산 등 추가 효과
        if (IsMaxLevel)
        {
            // planet.SetStateValue(PlanetStatType.SpreadShot, true);
        }
    }
}

public class InGameSkillMultiShot : InGameBaseSkill
{
    public InGameSkillMultiShot() { }

    public override void Apply(InGamePlanet planet)
    {
        // 스킬 효과: 한 번에 n발 발사 (레벨에 따라 증가 가능)
        planet.PlanetData.SetStateValue(PlanetStatType.ShotCount, MetaData.FirstValue + Level);

        // 스페셜 효과: 관통 혹은 각도 확산 등 추가 효과
        if (IsMaxLevel)
        {
            // planet.SetStateValue(PlanetStatType.SpreadShot, true);
        }
    }
}

public class InGameSkillAttackSpeed : InGameBaseSkill
{
    public InGameSkillAttackSpeed() { }

    public override void Apply(InGamePlanet planet)
    {

        planet.PlanetData.SetStateValue(PlanetStatType.AttackSpeed, planet.PlanetData.MetaData.AttackSpeed * (1 + MetaData.FirstValue * Level));

        // 스페셜 효과: 관통 혹은 각도 확산 등 추가 효과
        if (IsMaxLevel)
        {
            // planet.SetStateValue(PlanetStatType.SpreadShot, true);
        }
    }
}

public class InGameSkillAttackRange : InGameBaseSkill
{
    public InGameSkillAttackRange() { }

    public override void Apply(InGamePlanet planet)
    {
        planet.PlanetData.SetStateValue(PlanetStatType.Range, planet.PlanetData.MetaData.Range * (1 + MetaData.FirstValue * Level));

        // 스페셜 효과: 관통 혹은 각도 확산 등 추가 효과
        if (IsMaxLevel)
        {
            // planet.SetStateValue(PlanetStatType.SpreadShot, true);
        }
    }
}

public class InGameSkillHp : InGameBaseSkill
{
    public InGameSkillHp() { }

    public override void Apply(InGamePlanet planet)
    {

        planet.PlanetData.SetStateValue(PlanetStatType.MaxHp, planet.PlanetData.MetaData.Hp * (1 + MetaData.FirstValue * Level));

        // 스페셜 효과: 관통 혹은 각도 확산 등 추가 효과
        if (IsMaxLevel)
        {
            // planet.SetStateValue(PlanetStatType.SpreadShot, true);
        }
    }
}
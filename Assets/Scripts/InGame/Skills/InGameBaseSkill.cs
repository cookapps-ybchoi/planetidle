using System;
using UnityEngine;

public abstract class InGameBaseSkill : IInGameSkill
{

    public static InGameBaseSkill CreateSkill(InGameSkillId skillId)
    {
        InGameSkillMetaData skillMetaData = DataManager.Instance.InGameSkillDataList.Find(x => x.Id == skillId);
        if (skillMetaData == null)
        {
            throw new ArgumentException($"Unknown skill: {skillId}");
        }

        return skillId switch
        {
            InGameSkillId.MultiShot => new InGameSkillMultiShot().SetSkillMetaData(skillMetaData),
            InGameSkillId.AttackSpeed => new InGameSkillAttackSpeed().SetSkillMetaData(skillMetaData),
            InGameSkillId.AttackPower => new InGameSkillAttackPower().SetSkillMetaData(skillMetaData),
            InGameSkillId.AttackRange => new InGameSkillAttackRange().SetSkillMetaData(skillMetaData),
            InGameSkillId.Hp => new InGameSkillHp().SetSkillMetaData(skillMetaData),
            _ => throw new ArgumentException($"Unknown skill: {skillId}")
        };
    }

    public InGameBaseSkill SetSkillMetaData(InGameSkillMetaData skillMetaData)
    {
        SkillMetaData = skillMetaData;
        return this;
    }

    public InGameSkillMetaData SkillMetaData { get; protected set; }

    public InGameSkillId Id => SkillMetaData.Id;
    public InGameSkillType Type => SkillMetaData.Type;
    public int Level { get; protected set; } = 1;

    public virtual bool IsMaxLevel => Level >= SkillMetaData.MaxLevel;

    public virtual void LevelUp()
    {
        if (!IsMaxLevel)
            Level++;
    }

    public abstract void Apply(InGamePlanet planet);
}
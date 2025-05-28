using UnityEngine;
using System.Collections.Generic;
public class InGameSkillManager : GameObjectSingleton<InGameSkillManager>
{

    private Dictionary<InGameSkillId, IInGameSkill> _learnedSkills = new();

    protected void Start()
    {
        InGameEventManager.Instance.OnChoiceSkill += OnChoiceSkill;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnChoiceSkill -= OnChoiceSkill;
        }
    }

    private void OnChoiceSkill(InGameSkillId skillName)
    {
        if (_learnedSkills.TryGetValue(skillName, out var skill))
        {
            skill.LevelUp();
        }
        else
        {
            skill = InGameBaseSkill.CreateSkill(skillName);
            _learnedSkills.Add(skillName, skill);
        }

        skill.Apply(InGameManager.Instance.Planet);
    }
}

using UnityEngine;
using System.Collections.Generic;
using UnityEngine.Pool;
using System.Linq;

public class InGameSkillManager : GameObjectSingleton<InGameSkillManager>
{

    private Dictionary<InGameSkillId, IInGameSkill> _learnedSkills = new();

    protected override void Awake()
    {
        base.Awake();
        InGameEventHandler.OnChoiceSkill += OnChoiceSkill;
    }

    protected override void OnDestroy()
    {
        base.OnDestroy();
        InGameEventHandler.OnChoiceSkill -= OnChoiceSkill;
    }

    public void ResetSkills()
    {
        _learnedSkills.Clear();
    }

    public List<InGameBaseSkill> GetLearnedSkills()
    {
        return _learnedSkills.Values.Select(skill => skill as InGameBaseSkill).ToList();
    }

    // 랜덤으로 스킬을 선택
    // 이미 배운 스킬 중 MaxLevel인 스킬은 제외
    // DataManager에서 가져온 InGameSkillMetaData 에서 아직 배우지 않은 스킬만 선택
    public List<InGameBaseSkill> PickSkills(int pickCount = 3)
    {
        var pool = ListPool<InGameBaseSkill>.Get();
        
        try
        {
            // MaxLevel이 아닌 스킬들만 풀에 추가
            foreach (var skill in _learnedSkills.Values)
            {
                if (!skill.IsMaxLevel)
                {
                    pool.Add(skill as InGameBaseSkill);
                }
            }

            // 아직 배우지 않은 스킬들도 풀에 추가
            var allSkillMetaData = DataManager.Instance.InGameSkillDataList;
            foreach (var skillMetaData in allSkillMetaData)
            {
                if (!_learnedSkills.ContainsKey(skillMetaData.Id))
                {
                    var newSkill = InGameBaseSkill.CreateSkill(skillMetaData.Id);
                    pool.Add(newSkill);
                }
            }

            // 선택할 수 있는 스킬이 없는 경우 빈 리스트 반환
            if (pool.Count == 0)
            {
                return new List<InGameBaseSkill>();
            }

            // 실제 선택할 개수 계산 (가용 스킬 수와 요청된 개수 중 작은 값)
            int actualPickCount = Mathf.Min(pickCount, pool.Count);
            
            // 결과를 저장할 리스트 생성
            var result = new List<InGameBaseSkill>(actualPickCount);
            
            // 랜덤하게 스킬 선택
            for (int i = 0; i < actualPickCount; i++)
            {
                int randomIndex = Random.Range(0, pool.Count);
                result.Add(pool[randomIndex]);
                pool.RemoveAt(randomIndex);
            }

            return result;
        }
        finally
        {
            // 풀 반환
            ListPool<InGameBaseSkill>.Release(pool);
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
            skill.LevelUp();
            _learnedSkills.Add(skillName, skill);
        }

        skill.Apply(InGameManager.Instance.Planet);
    }
}

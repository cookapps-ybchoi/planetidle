using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameSkillItem : MonoBehaviour
{
    [SerializeField] private Image _skillIcon;
    [SerializeField] private Image _skillBorder;
    [SerializeField] private Image[] _skillLevelIcons;

    private InGameBaseSkill _skill;

    public async void SetSkill(InGameBaseSkill skill)
    {
        gameObject.SetActive(true);
        
        _skill = skill;

        _skillIcon.sprite = await ImageLoader.LoadIcon(skill.MetaData.Id);
        _skillBorder.color = skill.MetaData.Type == InGameSkillType.Passive ? ColorConstants.UI_SKILL_PASSIVE_BORDER_COLOR : ColorConstants.UI_SKILL_ACTIVE_BORDER_COLOR;

        for (int i = 0; i < _skillLevelIcons.Length; i++)
        {
            _skillLevelIcons[i].enabled = i < _skill.Level;
        }
    }
}

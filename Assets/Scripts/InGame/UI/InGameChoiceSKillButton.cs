using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class InGameChoiceSkillButton : BaseUI
{
    [SerializeField] private TextMeshProUGUI _skillName;
    [SerializeField] private TextMeshProUGUI _skillDescription;
    [SerializeField] private Image _skillIcon;
    [SerializeField] private Image _skillBorder;
    [SerializeField] private Image[] _skillLevelIcons;
    [SerializeField] private Button _button;

    private InGameBaseSkill _skill;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        InGameEventHandler.InvokeChoiceSkill(_skill.Id);
    }

    public async void SetSkill(InGameBaseSkill skill)
    {
        _skill = skill;

        // 스킬 선택 시 예정 레벨을 표시해야 하므로 레벨을 1 증가
        int skillLevel = skill.Level + 1;
        _skillName.text = $"{skill.MetaData.Name} Lv. {skillLevel}";
        _skillDescription.text = skill.MetaData.Description;
        _skillIcon.sprite = await ImageLoader.LoadIcon(skill.MetaData.Id);
        _skillBorder.color = skill.MetaData.Type == InGameSkillType.Passive ? ColorConstants.UI_SKILL_PASSIVE_BORDER_COLOR : ColorConstants.UI_SKILL_ACTIVE_BORDER_COLOR;

        for (int i = 0; i < _skillLevelIcons.Length; i++)
        {
            _skillLevelIcons[i].enabled = i < skillLevel;
        }
    }
}

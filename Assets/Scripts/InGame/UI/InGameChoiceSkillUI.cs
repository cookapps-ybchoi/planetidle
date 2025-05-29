using DG.Tweening;
using UnityEngine;

public class InGameChoiceSkillUI : BaseUI
{
    [SerializeField] private InGameChoiceSkillButton[] _skillButtons;
    [SerializeField] private InGameChoiceCurrentSkillItem[] _currentSkillItems;

    public override void Show(bool usingScale = true)
    {
        base.Show(usingScale);

        InitCurrentSkillItems();
        InitSkillButtons();
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void InitCurrentSkillItems()
    {
        var skills = InGameSkillManager.Instance.GetLearnedSkills();
        for (int i = 0; i < _currentSkillItems.Length; i++)
        {
            if (i < skills.Count)
            {
                _currentSkillItems[i].SetSkill(skills[i]);
            }
            else
            {
                _currentSkillItems[i].gameObject.SetActive(false);
            }
        }
    }

    private void InitSkillButtons()
    {
        var skills = InGameSkillManager.Instance.PickSkills();
        for (int i = 0; i < _skillButtons.Length; i++)
        {
            if (i < skills.Count)
            {
                _skillButtons[i].SetSkill(skills[i]);
            }
            else
            {
                _skillButtons[i].gameObject.SetActive(false);
            }
        }
    }
}

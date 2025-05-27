using DG.Tweening;
using UnityEngine;

public class InGameChoiceSkillUI : BaseUI
{
    [SerializeField] private InGameChoiceSKillButton[] _skillButtons;

    public override void Show(bool usingScale = true)
    {
        base.Show(usingScale);
    }

    public override void Hide()
    {
        base.Hide();
    }
}

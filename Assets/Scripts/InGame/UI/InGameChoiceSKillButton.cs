using UnityEngine;
using UnityEngine.UI;
public class InGameChoiceSKillButton : BaseUI
{
    [SerializeField] private InGameSkillId _skillId;

    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        InGameEventManager.Instance.InvokeChoiceSkill(_skillId);
    }
}

using UnityEngine;
using UnityEngine.UI;
public class InGameChoiceSKillButton : BaseUI
{
    [SerializeField] private int _index;

    [SerializeField] private Button _button;

    private void Start()
    {
        _button.onClick.AddListener(OnClick);
    }

    public void OnClick()
    {
        InGameEventManager.Instance.InvokeChoiceSkill(_index);
    }
}

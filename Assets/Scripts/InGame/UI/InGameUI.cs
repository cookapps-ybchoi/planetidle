using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InGameUI : GameObjectSingleton<InGameUI>
{
    [SerializeField] private InGameStartUI _startUI;
    [SerializeField] private InGamePlayUI _playUI;
    [SerializeField] private InGameResultUI _resultUI;
    [SerializeField] private InGameChoiceSkillUI _choiceSkillUI;

    protected override void Awake()
    {
        base.Awake();
        _playUI.gameObject.SetActive(false);
        _startUI.gameObject.SetActive(false);
        _resultUI.gameObject.SetActive(false);
        _choiceSkillUI.gameObject.SetActive(false);

        InGameEventHandler.OnGameStateChanged += OnGameStateChanged;
        InGameEventHandler.OnLevelChanged += OnLevelChanged;
        InGameEventHandler.OnChoiceSkill += OnChoiceSkill;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        InGameEventHandler.OnGameStateChanged -= OnGameStateChanged;
        InGameEventHandler.OnLevelChanged -= OnLevelChanged;
        InGameEventHandler.OnChoiceSkill -= OnChoiceSkill;
    }

    private void OnGameStateChanged(InGameState state)
    {
        if (state == InGameState.GameReady)
        {
            _startUI.Show();
        }
        else if (state == InGameState.GamePlay)
        {
            _playUI.Show();
        }
        else if (state == InGameState.GameOver)
        {
            _playUI.Hide();
            _resultUI.Show();
        }
    }

    private void OnLevelChanged(int level)
    {
        if (level > 1)
        {
            _choiceSkillUI.Show();
        }
    }

    private void OnChoiceSkill(InGameSkillId skillName)
    {
        _choiceSkillUI.Hide();
        InGameManager.Instance.ResumeGame();
    }
}

using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InGameUI : GameObjectSingleton<InGameUI>
{
    [SerializeField] private InGameStartUI _startUI;
    [SerializeField] private InGamePlayUI _playUI;
    [SerializeField] private InGameResultUI _resultUI;
    [SerializeField] private InGameChoiceSkillUI _choiceSkillUI;

    private void Start()
    {
        _playUI.gameObject.SetActive(false);
        _startUI.gameObject.SetActive(false);
        _resultUI.gameObject.SetActive(false);
        _choiceSkillUI.gameObject.SetActive(false);

        InGameEventManager.Instance.OnGameStateChanged += OnGameStateChanged;
        InGameEventManager.Instance.OnLevelChanged += OnLevelChanged;
        InGameEventManager.Instance.OnChoiceSkill += OnChoiceSkill;
    }
    protected override void OnDestroy()
    {
        base.OnDestroy();
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            InGameEventManager.Instance.OnLevelChanged -= OnLevelChanged;
            InGameEventManager.Instance.OnChoiceSkill -= OnChoiceSkill;
        }
    }

    private void OnGameStateChanged(InGameState state)
    {
        if (state == InGameState.GameReady)
        {
            _resultUI.Hide();
            _startUI.Show();
        }
        else if (state == InGameState.GamePlay)
        {
            _playUI.Show();
            _startUI.Hide();
        }
        else if (state == InGameState.GameOver)
        {
            _playUI.Hide();
            _resultUI.Show();
        }
    }

    private void OnLevelChanged(int level)
    {
        _choiceSkillUI.Show();
    }

    private void OnChoiceSkill(int skillId)
    {
        _choiceSkillUI.Hide();
        InGameManager.Instance.ResumeGame();
    }
}

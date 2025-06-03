using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class InGamePlayUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _killCountText;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _currentLevelText;
    [SerializeField] private TextMeshProUGUI _nextLevelText;
    [SerializeField] private TextMeshProUGUI _hpText;
    [SerializeField] private Slider _levelSlider;
    [SerializeField] private Slider _bossHpSlider;
    [SerializeField] private Slider _hpSlider;
    [SerializeField] private InGameSkillItem[] _currentSkillItems;

    private double _hp = 0;
    private double _maxHp = 0;

    protected override void Awake()
    {
        base.Awake();
        InGameEventHandler.OnTimeChanged += OnTimeChanged;
        InGameEventHandler.OnRecordDataChanged += OnRecordDataChanged;
        InGameEventHandler.OnLevelChanged += OnLevelChanged;
        InGameEventHandler.OnExpChanged += OnExpChanged;
        InGameEventHandler.OnPlanetStateValueChanged += OnPlanetStateValueChanged;
        InGameEventHandler.OnChoiceSkill += OnChoiceSkill;
        InGameEventHandler.OnBossWaveStarted += OnBossWaveStarted;
        InGameEventHandler.OnBossStateChanged += OnBossStateChanged;
        InGameEventHandler.OnBossWaveCompleted += OnBossWaveCompleted;
    }

    protected void OnDestroy()
    {
        InGameEventHandler.OnTimeChanged -= OnTimeChanged;
        InGameEventHandler.OnRecordDataChanged -= OnRecordDataChanged;
        InGameEventHandler.OnLevelChanged -= OnLevelChanged;
        InGameEventHandler.OnExpChanged -= OnExpChanged;
        InGameEventHandler.OnPlanetStateValueChanged -= OnPlanetStateValueChanged;
        InGameEventHandler.OnChoiceSkill -= OnChoiceSkill;
        InGameEventHandler.OnBossWaveStarted -= OnBossWaveStarted;
        InGameEventHandler.OnBossStateChanged -= OnBossStateChanged;
        InGameEventHandler.OnBossWaveCompleted -= OnBossWaveCompleted;
    }

    public override void Hide()
    {
        base.Hide();
        ResetUI();
    }

    public void ResetUI()
    {
        _currentLevelText.text = "1";
        _nextLevelText.text = "2";

        _levelSlider.value = 0;
        _hpSlider.value = 0;

        _timeText.text = "00:00";
        _killCountText.text = "0";
        _coinText.text = "0";
        _hpText.text = string.Empty;

        _bossHpSlider.gameObject.SetActive(false);
        _levelSlider.gameObject.SetActive(true);

        for (int i = 0; i < _currentSkillItems.Length; i++)
        {
            _currentSkillItems[i].gameObject.SetActive(false);
        }
    }

    private void OnChoiceSkill(InGameSkillId skillId)
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

    private void OnTimeChanged(float totalPlayTime)
    {
        //mm:ss 형식으로 표시
        int minutes = Mathf.FloorToInt(totalPlayTime / 60);
        int seconds = Mathf.FloorToInt(totalPlayTime % 60);
        _timeText.text = $"{minutes:D2}:{seconds:D2}";
    }

    private void OnRecordDataChanged(RecordData recordData)
    {
        _killCountText.text = $"{recordData.TotalEnemiesDestroyed}";
        _coinText.text = $"{recordData.TotalCoinEarned}";
    }

    private void OnLevelChanged(int level)
    {
        _currentLevelText.text = $"{level}";
        _nextLevelText.text = $"{level + 1}";
        _levelSlider.DOKill();
        _levelSlider.value = 0;
    }

    private void OnExpChanged(int currentExp, int maxExp)
    {
        if (currentExp == 0 || maxExp == 0)
        {
            _levelSlider.value = 0;
        }
        else
        {
            float targetValue = (float)currentExp / (float)maxExp;
            if (_levelSlider.value < targetValue)
            {
                _levelSlider.DOValue(targetValue, 0.2f).SetUpdate(true);
            }
            else
            {
                _levelSlider.value = targetValue;
            }
        }
    }

    private void OnPlanetStateValueChanged(PlanetStatType statType, double value)
    {
        if (statType == PlanetStatType.Hp)
        {
            _hp = value;
        }
        else if (statType == PlanetStatType.MaxHp)
        {
            _maxHp = value;
        }


        if (_maxHp > 0)
        {
            _hpText.text = $"{_hp}/{_maxHp}";
            _hpSlider.DOValue((float)_hp / (float)_maxHp, 0.2f).SetUpdate(true);
        }
        else
        {
            _hpSlider.value = 0;
            _hpText.text = string.Empty;
        }
    }

    private void OnBossStateChanged(EnemyData enemyData)
    {
        if (enemyData.Entity.type == EnemyType.Boss)
        {
            _bossHpSlider.DOValue((float)enemyData.CurHp / (float)enemyData.MaxHp, 0.2f).SetUpdate(true);
        }
    }

    private void OnBossWaveStarted(EnemyData enemyData)
    {
        if (enemyData.Entity.type == EnemyType.Boss)
        {
            _bossHpSlider.gameObject.SetActive(true);
            _bossHpSlider.value = 0;
            _bossHpSlider.DOValue(1, 0.2f).SetUpdate(true);
            _levelSlider.gameObject.SetActive(false);
        }
    }

    private void OnBossWaveCompleted(EnemyData enemyData)
    {
        if (enemyData.Entity.type == EnemyType.Boss)
        {
            _bossHpSlider.gameObject.SetActive(false);
            _levelSlider.gameObject.SetActive(true);
        }
    }
}

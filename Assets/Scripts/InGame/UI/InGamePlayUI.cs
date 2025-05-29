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
    [SerializeField] private Slider _hpSlider;

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
    }

    protected void OnDestroy()
    {
        InGameEventHandler.OnTimeChanged -= OnTimeChanged;
        InGameEventHandler.OnRecordDataChanged -= OnRecordDataChanged;
        InGameEventHandler.OnLevelChanged -= OnLevelChanged;
        InGameEventHandler.OnExpChanged -= OnExpChanged;
        InGameEventHandler.OnPlanetStateValueChanged -= OnPlanetStateValueChanged;
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
    }

    private void OnExpChanged(int currentExp, int maxExp)
    {
        if (maxExp == 0)
        {
            _levelSlider.value = 0;
        }
        else
        {
            _levelSlider.DOValue((float)currentExp / (float)maxExp, 0.2f).SetUpdate(true);
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
}

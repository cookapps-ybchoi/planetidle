using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DG.Tweening;
public class InGamePlayUI : BaseUI
{
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _killCountText;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _levelSlider;

    private float _targetSliderValue = 0;
    private float _currentSliderValue = 0;

    private void Start()
    {
        InGameEventManager.Instance.OnTimeChanged += OnTimeChanged;
        InGameEventManager.Instance.OnRecordDataChanged += OnRecordDataChanged;
        InGameEventManager.Instance.OnLevelChanged += OnLevelChanged;
        InGameEventManager.Instance.OnExpChanged += OnExpChanged;
    }

    private void Update()
    {
        if (_currentSliderValue != _targetSliderValue)  // 값이 다를 때만 업데이트
        {
            _currentSliderValue = Mathf.MoveTowards(_currentSliderValue, _targetSliderValue, Time.deltaTime);
            _levelSlider.value = _currentSliderValue;
        }
    }

    private void OnDestroy()
    {
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnTimeChanged -= OnTimeChanged;
            InGameEventManager.Instance.OnRecordDataChanged -= OnRecordDataChanged;
            InGameEventManager.Instance.OnLevelChanged -= OnLevelChanged;
            InGameEventManager.Instance.OnExpChanged -= OnExpChanged;
        }
    }

    public override void Show(bool usingScale = true)
    {
        base.Show(usingScale);
    }

    public override void Hide()
    {
        base.Hide();
        ResetUI();
    }

    public void ResetUI()
    {
        _levelText.text = "1";
        _levelSlider.value = 0;
        _targetSliderValue = 0;
        _currentSliderValue = 0;
        _timeText.text = "00:00";
        _killCountText.text = "0";
        _coinText.text = "0";
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
        _levelText.text = $"{level}";
        _levelSlider.value = 0;
        _currentSliderValue = 0;
        _targetSliderValue = 0;
    }

    private void OnExpChanged(int currentExp, int maxExp)
    {
        if (maxExp == 0)
        {
            _targetSliderValue = 0;
        }
        else
        {
            _targetSliderValue = (float)currentExp / (float)maxExp;
        }
    }

}

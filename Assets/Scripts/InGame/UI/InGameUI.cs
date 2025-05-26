using UnityEngine;
using TMPro;
using UnityEngine.UI;
public class InGameUI : MonoBehaviour
{


    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _killCountText;
    [SerializeField] private TextMeshProUGUI _coinText;
    [SerializeField] private TextMeshProUGUI _levelText;
    [SerializeField] private Slider _levelSlider;

    private void Start()
    {
        _levelSlider.maxValue = 0;
        _levelSlider.value = 0;
        _timeText.text = "00:00";
        _killCountText.text = "0";
        _coinText.text = "0";
        _levelText.text = "1";

        InGameEventManager.Instance.OnTimeChanged += OnTimeChanged;
        InGameEventManager.Instance.OnRecordDataChanged += OnRecordDataChanged;
        InGameEventManager.Instance.OnLevelChanged += OnLevelChanged;
        InGameEventManager.Instance.OnPointsChanged += OnPointsChanged;

    }

    private void OnDestroy()
    {
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnTimeChanged -= OnTimeChanged;
            InGameEventManager.Instance.OnRecordDataChanged -= OnRecordDataChanged;
            InGameEventManager.Instance.OnLevelChanged -= OnLevelChanged;
            InGameEventManager.Instance.OnPointsChanged -= OnPointsChanged;
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
        _levelText.text = $"{level}";
    }

    private void OnPointsChanged(int currentPoints, int maxPoints)
    {
        _levelSlider.maxValue = maxPoints;
        _levelSlider.value = currentPoints;
    }
}

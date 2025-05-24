using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UpgradeUI : MonoBehaviour
{
    [SerializeField] private Button[] _upgradeButtons;
    [SerializeField] private TextMeshProUGUI[] _costTexts;
    [SerializeField] private TextMeshProUGUI _pointText;

    private void Start()
    {
        InitializeUI();
        InGameEventManager.Instance.OnPointsChanged += UpdatePointText;
    }

    private void OnDestroy()
    {
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnPointsChanged -= UpdatePointText;
        }
    }

    private void InitializeUI()
    {
        for (int i = 0; i < _upgradeButtons.Length; i++)
        {
            PlanetStatType statType = (PlanetStatType)i;
            _upgradeButtons[i].onClick.AddListener(() => OnUpgradeButtonClicked(statType));
            UpdateCostText(statType);
        }
    }

    private void OnUpgradeButtonClicked(PlanetStatType statType)
    {
        if (InGameManager.Instance.TryUpgradeStat(statType))
        {
            UpdateCostText(statType);
        }
    }

    private void UpdateCostText(PlanetStatType statType)
    {
        int cost = InGameManager.Instance.GetUpgradeCost(statType);
        _costTexts[(int)statType].text = $"Cost: {cost}";
    }

    private void UpdatePointText(int points)
    {
        _pointText.text = $"Points: {points}";
    }
}
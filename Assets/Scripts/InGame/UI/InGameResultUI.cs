using UnityEngine;
using DG.Tweening;
using TMPro;
public class InGameResultUI : BaseUI
{
    [SerializeField] private GameObject _victoryObj;
    [SerializeField] private GameObject _defeatObj;
    [SerializeField] private GameObject _timeObj;
    [SerializeField] private GameObject _bestObj;

    [SerializeField] private TextMeshProUGUI _recordText;
    [SerializeField] private TextMeshProUGUI _timeText;
    [SerializeField] private TextMeshProUGUI _killText;
    [SerializeField] private TextMeshProUGUI _continueText;

    public override void Show(bool usingScale = true)
    {
        base.Show(usingScale);

        _continueText.transform.DOMoveY(_continueText.transform.position.y + 5f, 1f)
            .SetEase(Ease.InOutSine)
            .SetLoops(-1, LoopType.Yoyo)
            .SetUpdate(true);
    }

    public override void Hide()
    {
        base.Hide();
    }

    //화면 아무데나 터치하면 게임 준비 호출
    public void OnTouch()
    {
        InGameManager.Instance.ReadyToStart();
        Hide();
    }
}

using UnityEngine;
using DG.Tweening;
public class InGameStartUI : BaseUI
{
    public override void Show(bool usingScale = true)
    {
        base.Show(usingScale);
    }

    public override void Hide()
    {
        base.Hide();
    }

    public void OnClickStart()
    {
        InGameManager.Instance.StartGame();
        Hide();
    }
}

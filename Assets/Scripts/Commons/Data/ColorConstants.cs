using UnityEngine;
using DG.Tweening;

public class ColorConstants
{
    public static readonly Color UI_SKILL_PASSIVE_BORDER_COLOR = ColorUtility.TryParseHtmlString("#EEBD00", out Color color) ? color : Color.white;
    public static readonly Color UI_SKILL_ACTIVE_BORDER_COLOR = ColorUtility.TryParseHtmlString("#555555", out Color color) ? color : Color.white;
}
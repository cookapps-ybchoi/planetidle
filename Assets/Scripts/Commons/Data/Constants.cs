using UnityEngine;
using DG.Tweening;

public class Constants
{
    public const int DEFAULT_ID = 1;
    public const string PLANET_SAVE_PATH = "planetData.json";
    public const string INGAME_SKILL_ICON_NAME_PREFIX = "skill_icon_";


    //행성의 기본 능력 
    public const double PLANET_ATTACK_POWER_DEFUALT = 10f;
    public const double PLANET_ATTACK_COOLTIME_DEFUALT = 0.5f;
    public const double PLANET_ATTACK_SPEED_DEFUALT = 1f;
    public const double PLANET_RANGE_DEFUALT = 1.5f;
    public const double PLANET_HP_DEFAULT = 20f;
    public const double PLANET_HP_RECOVERY_DEFAULT = 0f;
    public const double PLANET_SHOT_COUNT_DEFAULT = 1f;

    //게임 환경 변수
    public const float PLANET_BULLET_SPEED = 6f;
    public const float ENEMY_SPAWN_DISTANCE = 5f;
    public const float ENEMY_SPAWN_DISTANCE_BOSS = 6f;

    //UI 변수
    public const float UI_FADE_TIME = 0.3f;
    public const float UI_FADE_DELAY = 0.3f;
    public const Ease UI_FADE_EASE_DEFAULT = Ease.OutQuad;

    public static readonly Color UI_SKILL_PASSIVE_BORDER_COLOR = ColorUtility.TryParseHtmlString("#EEBD00", out Color color) ? color : Color.white;
    public static readonly Color UI_SKILL_ACTIVE_BORDER_COLOR = ColorUtility.TryParseHtmlString("#555555", out Color color) ? color : Color.white;
}
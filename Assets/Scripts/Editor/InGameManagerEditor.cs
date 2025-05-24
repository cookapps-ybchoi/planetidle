using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InGameManager))]
public class InGameManagerEditor : Editor
{
    private readonly Color buttonColor = new Color(0.2f, 0.6f, 1f);
    private readonly Color disabledButtonColor = new Color(0.5f, 0.5f, 0.5f);
    private readonly int maxLevel = 100; // 최대 레벨 설정

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        // 플레이 모드가 아닐 때는 메뉴를 표시하지 않음
        if (!Application.isPlaying)
        {
            EditorGUILayout.HelpBox("게임을 실행하면 메뉴가 표시됩니다.", MessageType.Info);
            return;
        }

        InGameManager inGameManager = (InGameManager)target;

        EditorGUILayout.BeginHorizontal();
        if (GUILayout.Button("게임 시작")) { inGameManager.StartGame(); }
        if (GUILayout.Button("일시 정지")) { inGameManager.PauseGame(); }
        if (GUILayout.Button("게임 재개")) { inGameManager.ResumeGame(); }
        if (GUILayout.Button("게임 종료")) { inGameManager.GameOver(); }
        EditorGUILayout.EndHorizontal();

        if (inGameManager.IsPlaying)
        {
            // 현재 포인트 표시
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("현재 포인트", inGameManager.CurrentPoints.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.LabelField("현재 체력", inGameManager.GetCurrentPlanetHp().ToString("F1"), EditorStyles.boldLabel);
            EditorGUILayout.EndHorizontal();

            // 현재 웨이브 표시
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("현재 웨이브", InGameWaveManager.Instance.CurrentWave.ToString(), EditorStyles.boldLabel);
            float progress = InGameWaveManager.Instance.GetCurrentWaveProgress();
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(false, 20), progress, $"{progress:P1}");
            EditorGUILayout.EndHorizontal();
            
            // 레벨업 버튼들 표시
            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("스탯 레벨업", EditorStyles.boldLabel);

            EditorGUILayout.BeginHorizontal();
            DrawStatUpgradeButton(inGameManager, PlanetStatType.AttackPower, "공격력");
            DrawStatUpgradeButton(inGameManager, PlanetStatType.AttackSpeed, "공격속도");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            DrawStatUpgradeButton(inGameManager, PlanetStatType.Hp, "체력");
            DrawStatUpgradeButton(inGameManager, PlanetStatType.HpRecovery, "체력 회복");
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.Space(10);
            EditorGUILayout.LabelField("게임 컨트롤", EditorStyles.boldLabel);
        }
    }

    private void DrawStatUpgradeButton(InGameManager inGameManager, PlanetStatType statType, string statName)
    {
        int currentLevel = inGameManager.GetPlanetStateLevel(statType);
        double currentValue = inGameManager.GetPlanetStateValue(statType);
        double nextValue = inGameManager.GetPlanetNextLevelStateValue(statType);
        double upgradeCost = inGameManager.GetUpgradeCost(statType);

        EditorGUILayout.BeginVertical(EditorStyles.helpBox);

        // 현재 레벨과 값 표시
        EditorGUILayout.LabelField($"{statName} (Lv.{currentLevel}/{maxLevel})", EditorStyles.boldLabel);
        EditorGUILayout.LabelField($"현재 값: {currentValue:F1}");
        if (currentLevel < maxLevel)
        {
            EditorGUILayout.LabelField($"다음 레벨: {nextValue:F1}");
            EditorGUILayout.LabelField($"업그레이드 비용: {upgradeCost:N0} 포인트");
        }

        // 레벨업 버튼
        GUI.backgroundColor = currentLevel >= maxLevel ? disabledButtonColor : buttonColor;
        EditorGUI.BeginDisabledGroup(currentLevel >= maxLevel || inGameManager.CurrentPoints < upgradeCost);
        if (GUILayout.Button($"레벨업 (Lv.{currentLevel} → Lv.{currentLevel + 1})"))
        {
            inGameManager.TryUpgradeStat(statType);
        }
        EditorGUI.EndDisabledGroup();
        GUI.backgroundColor = Color.white;

        EditorGUILayout.EndVertical();
    }
}
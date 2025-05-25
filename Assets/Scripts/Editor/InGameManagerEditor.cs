using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InGameManager))]
public class InGameManagerEditor : Editor
{
    private readonly Color buttonColor = new Color(0.2f, 0.6f, 1f);
    private readonly Color disabledButtonColor = new Color(0.5f, 0.5f, 0.5f);
    private readonly int maxLevel = 100; // 최대 레벨 설정

    private void OnEnable()
    {
        // 에디터가 활성화될 때 이벤트 구독
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnGameStateChanged += OnGameStateChanged;
            InGameEventManager.Instance.OnPointsChanged += OnPointsChanged;
            InGameEventManager.Instance.OnPlanetStateLevelChanged += OnPlanetStateLevelChanged;
            InGameEventManager.Instance.OnWaveComplete += OnWaveComplete;
            InGameEventManager.Instance.OnWaveWaitProgressChanged += OnWaveWaitProgressChanged;
            InGameEventManager.Instance.OnEnemyDestroyed += OnEnemyDestroyed;
        }
    }

    private void OnDisable()
    {
        // 에디터가 비활성화될 때 이벤트 구독 해제
        if (InGameEventManager.Instance != null)
        {
            InGameEventManager.Instance.OnGameStateChanged -= OnGameStateChanged;
            InGameEventManager.Instance.OnPointsChanged -= OnPointsChanged;
            InGameEventManager.Instance.OnPlanetStateLevelChanged -= OnPlanetStateLevelChanged;
            InGameEventManager.Instance.OnWaveComplete -= OnWaveComplete;
            InGameEventManager.Instance.OnWaveWaitProgressChanged -= OnWaveWaitProgressChanged;
            InGameEventManager.Instance.OnEnemyDestroyed -= OnEnemyDestroyed;
        }
    }

    private void OnGameStateChanged(InGameState state)
    {
        // 게임 상태가 변경될 때마다 에디터 UI 갱신
        Repaint();
    }

    private void OnPointsChanged(int points, int totalPoints)
    {
        // 포인트가 변경될 때마다 에디터 UI 갱신
        Repaint();
    }

    private void OnPlanetStateLevelChanged(PlanetStatType statType, int level)
    {
        // 행성 상태 레벨이 변경될 때마다 에디터 UI 갱신
        Repaint();
    }

    private void OnWaveComplete(int waveLevel)
    {
        // 웨이브가 완료될 때마다 에디터 UI 갱신
        Repaint();
    }

    private void OnWaveWaitProgressChanged(float progress)
    {
        // 웨이브 대기 진행률이 변경될 때마다 에디터 UI 갱신
        Repaint();
    }

    private void OnEnemyDestroyed(int enemyId)
    {
        // 적이 처치될 때마다 에디터 UI 갱신
        Repaint();
    }

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
            EditorGUILayout.EndHorizontal();
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("현재 체력", EditorStyles.boldLabel);
            float currentHp = (float)inGameManager.GetCurrentPlanetHp();
            float maxHp = (float)inGameManager.GetPlanetStateValue(PlanetStatType.Hp);
            float hpRatio = currentHp / maxHp;
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(false, 20), hpRatio, $"{currentHp:F1}/{maxHp:F1}");
            EditorGUILayout.EndHorizontal();

            // 현재 웨이브 표시
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("현재 웨이브", InGameWaveManager.Instance.CurrentWave.ToString(), EditorStyles.boldLabel);
            float progress = InGameWaveManager.Instance.GetCurrentWaveProgress();
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(false, 20), progress, $"{progress:P1}");
            EditorGUILayout.EndHorizontal();

            // 웨이브 대기 진행률 표시
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("웨이브 대기 진행률", InGameWaveManager.Instance.GetCurrentWaveWaitProgress().ToString("P1"), EditorStyles.boldLabel);
            progress = InGameWaveManager.Instance.GetCurrentWaveWaitProgress();
            EditorGUI.ProgressBar(EditorGUILayout.GetControlRect(false, 20), progress, $"{progress:P1}");
            EditorGUILayout.EndHorizontal();

            // 적 처치 횟수, 레벨업 횟수, 포인트 획득, 포인트 소모 표시
            EditorGUILayout.Space(10);
            EditorGUILayout.BeginVertical(EditorStyles.helpBox);
            EditorGUILayout.LabelField("게임 통계", EditorStyles.boldLabel);
            
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("적 처치 횟수", inGameManager.TotalEnemiesDestroyed.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.LabelField("레벨업 횟수", inGameManager.TotalPlanetLevelUpsCount.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            
            EditorGUILayout.BeginVertical();
            EditorGUILayout.LabelField("포인트 획득", inGameManager.TotalPointsEarned.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.LabelField("포인트 소모", inGameManager.TotalPointsSpent.ToString(), EditorStyles.boldLabel);
            EditorGUILayout.EndVertical();
            EditorGUILayout.EndHorizontal();
            
            EditorGUILayout.EndVertical();

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
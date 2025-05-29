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
        InGameEventHandler.OnGameStateChanged += OnGameStateChanged;
        InGameEventHandler.OnExpChanged += OnExpChanged;
        InGameEventHandler.OnEnemyDestroyed += OnEnemyDestroyed;
    }

    private void OnDisable()
    {
        // 에디터가 비활성화될 때 이벤트 구독 해제
        InGameEventHandler.OnGameStateChanged -= OnGameStateChanged;
        InGameEventHandler.OnExpChanged -= OnExpChanged;
        InGameEventHandler.OnEnemyDestroyed -= OnEnemyDestroyed;
    }

    private void OnGameStateChanged(InGameState state)
    {
        // 게임 상태가 변경될 때마다 에디터 UI 갱신
        Repaint();
    }

    private void OnExpChanged(int currentExp, int maxExp)
    {
        // 포인트가 변경될 때마다 에디터 UI 갱신
        Repaint();
    }

    private void OnEnemyDestroyed(int enemyId, bool isKilled)
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
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("현재 경험치", GUILayout.Width(100));
            EditorGUILayout.EndHorizontal();

            //타임스케일 조절
            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField("타임스케일", GUILayout.Width(100));
            float timeScale = EditorGUILayout.Slider(Time.timeScale, 0f, 5f);
            Time.timeScale = timeScale;
            EditorGUILayout.EndHorizontal();
        }
    }
}
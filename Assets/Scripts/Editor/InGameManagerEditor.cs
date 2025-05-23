using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(InGameManager))]
public class InGameManagerEditor : Editor
{
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

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("게임 관리", EditorStyles.boldLabel);

        if (GUILayout.Button("게임 시작"))
        {
            inGameManager.StartGame();
        }

        if (GUILayout.Button("게임 일시정지"))
        {
            inGameManager.PauseGame();
        }

        if (GUILayout.Button("게임 재개"))
        {
            inGameManager.ResumeGame();
        }

        if (GUILayout.Button("게임 종료"))
        {
            inGameManager.GameOver();
        }
    }
} 
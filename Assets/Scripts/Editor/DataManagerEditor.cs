using UnityEngine;
using UnityEditor;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DataManager dataManager = (DataManager)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("데이터 관리", EditorStyles.boldLabel);

        if (GUILayout.Button("저장 데이터 삭제"))
        {
            if (EditorUtility.DisplayDialog("저장 데이터 삭제",
                "정말로 모든 저장 데이터를 삭제하시겠습니까?\n이 작업은 되돌릴 수 없습니다.",
                "삭제", "취소"))
            {
                dataManager.DeleteSaveData();
            }
        }
    }
} 
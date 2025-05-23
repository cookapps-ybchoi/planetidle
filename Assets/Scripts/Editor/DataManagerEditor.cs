using UnityEngine;
using UnityEditor;
using System.Threading.Tasks;

[CustomEditor(typeof(DataManager))]
public class DataManagerEditor : Editor
{
    private bool _showPlanetData = false;
    private bool _showEnemyData = false;
    private bool _showWaveData = false;

    public override void OnInspectorGUI()
    {
        base.OnInspectorGUI();

        DataManager dataManager = (DataManager)target;

        EditorGUILayout.Space(10);
        EditorGUILayout.LabelField("데이터 관리", EditorStyles.boldLabel);

        // 행성 데이터 표시
        _showPlanetData = EditorGUILayout.Foldout(_showPlanetData, "행성 데이터");
        if (_showPlanetData && dataManager.PlanetData != null)
        {
            EditorGUI.indentLevel++;
            var planetData = dataManager.PlanetData;
            
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.LabelField("행성 ID", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("행성 레벨", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("공격력 레벨", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("공격속도 레벨", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("사정거리 레벨", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("체력 레벨", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.LabelField("체력회복 레벨", EditorStyles.boldLabel, GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();

            EditorGUILayout.BeginHorizontal();
            EditorGUILayout.LabelField(planetData.PlanetId.ToString(), GUILayout.Width(80));
            EditorGUILayout.LabelField(planetData.PlanetLevel.ToString(), GUILayout.Width(80));
            EditorGUILayout.LabelField(planetData.AttackPowerLevel.ToString(), GUILayout.Width(80));
            EditorGUILayout.LabelField(planetData.AttackSpeedLevel.ToString(), GUILayout.Width(80));
            EditorGUILayout.LabelField(planetData.RangeLevel.ToString(), GUILayout.Width(80));
            EditorGUILayout.LabelField(planetData.HpLevel.ToString(), GUILayout.Width(80));
            EditorGUILayout.LabelField(planetData.HpRecoveryLevel.ToString(), GUILayout.Width(80));
            EditorGUILayout.EndHorizontal();
            EditorGUI.indentLevel--;
        }

        // 적 데이터 표시
        _showEnemyData = EditorGUILayout.Foldout(_showEnemyData, "적 데이터");
        if (_showEnemyData && dataManager.EnemyDataList != null)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.LabelField("적 ID", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("레벨", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("체력", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("이동속도", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("공격범위", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("공격력", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("공격속도", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();

            foreach (var enemyData in dataManager.EnemyDataList)
            {
                EditorGUILayout.BeginHorizontal();
                EditorGUILayout.LabelField(enemyData.EnemyId.ToString(), GUILayout.Width(60));
                EditorGUILayout.LabelField(enemyData.EnemyLevel.ToString(), GUILayout.Width(60));
                EditorGUILayout.LabelField(enemyData.Hp.ToString("F1"), GUILayout.Width(60));
                EditorGUILayout.LabelField(enemyData.MoveSpeed.ToString("F1"), GUILayout.Width(60));
                EditorGUILayout.LabelField(enemyData.AttackRange.ToString("F1"), GUILayout.Width(60));
                EditorGUILayout.LabelField(enemyData.AttackPower.ToString("F1"), GUILayout.Width(60));
                EditorGUILayout.LabelField(enemyData.AttackDelay.ToString("F1"), GUILayout.Width(60));
                EditorGUILayout.EndHorizontal();
            }
            EditorGUI.indentLevel--;
        }

        // 웨이브 데이터 표시
        _showWaveData = EditorGUILayout.Foldout(_showWaveData, "웨이브 데이터");
        if (_showWaveData && dataManager.WaveDataList != null)
        {
            EditorGUI.indentLevel++;
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            EditorGUILayout.LabelField("웨이브 ID", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("웨이브 레벨", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("생성 횟수", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("배치 수", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("생성 간격", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("생성 ID", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.LabelField("생성 확률", EditorStyles.boldLabel, GUILayout.Width(60));
            EditorGUILayout.EndHorizontal();

            foreach (var waveData in dataManager.WaveDataList)
            {
                for (int i = 0; i < waveData.SpawnIds.Length; i++)
                {
                    EditorGUILayout.BeginHorizontal();
                    EditorGUILayout.LabelField(waveData.WaveId.ToString(), GUILayout.Width(60));
                    EditorGUILayout.LabelField(waveData.WaveLevel.ToString(), GUILayout.Width(60));
                    EditorGUILayout.LabelField(waveData.SpawnCount.ToString(), GUILayout.Width(60));
                    EditorGUILayout.LabelField(waveData.BatchCount.ToString(), GUILayout.Width(60));
                    EditorGUILayout.LabelField(waveData.SpawnInterval.ToString("F1"), GUILayout.Width(60));
                    EditorGUILayout.LabelField(waveData.SpawnIds[i].ToString(), GUILayout.Width(60));
                    EditorGUILayout.LabelField($"{waveData.SpawnRates[i] * 100:F1}%", GUILayout.Width(60));
                    EditorGUILayout.EndHorizontal();
                }
            }
            EditorGUI.indentLevel--;
        }

        EditorGUILayout.Space(10);
        if (GUILayout.Button("저장 데이터 삭제"))
        {
            if (EditorUtility.DisplayDialog("저장 데이터 삭제",
                "정말로 모든 저장 데이터를 삭제하시겠습니까?\n이 작업은 되돌릴 수 없습니다.",
                "삭제", "취소"))
            {
                DeleteSaveData(dataManager);
            }
        }
    }

    private async void DeleteSaveData(DataManager dataManager)
    {
        await dataManager.DeleteSaveDataAsync();
    }
}   
using UnityEngine;

public class DistanceGizmo2D : MonoBehaviour
{
    [SerializeField] private int maxDistance = 5; // 최대 거리 (0.5 단위로 곱해짐)
    [SerializeField] private Color gizmoColor = new Color(1f, 1f, 1f, 0.3f); // 기본 색상
    [SerializeField] private int segments = 32; // 원의 세그먼트 수
    [SerializeField] private float textSize = 0.2f; // 텍스트 크기
    [SerializeField] private Color textColor = Color.white; // 텍스트 색상

    private void OnDrawGizmos()
    {
        // 0.5 단위로 원 그리기
        for (int i = 1; i <= maxDistance; i++)
        {
            float radius = i * 0.5f;
            DrawCircle2D(transform.position, radius, gizmoColor);
        }
    }

    private void DrawCircle2D(Vector3 center, float radius, Color color)
    {
        Gizmos.color = color;
        int segments = 32;
        
        Vector3 previousPoint = center + new Vector3(radius, 0f, 0f);
        
        for (int i = 0; i <= segments; i++)
        {
            float angle = (float)i / segments * 360f * Mathf.Deg2Rad;
            Vector3 newPoint = center + new Vector3(
                Mathf.Cos(angle) * radius,
                Mathf.Sin(angle) * radius,
                0f
            );
            
            Gizmos.DrawLine(previousPoint, newPoint);
            previousPoint = newPoint;
        }

        // 원의 반지름 표시
        Vector3 textPosition = center + new Vector3(0f, radius + 0.2f, 0f);
        UnityEditor.Handles.color = textColor;
        UnityEditor.Handles.Label(textPosition, radius.ToString("F2"));
    }
} 
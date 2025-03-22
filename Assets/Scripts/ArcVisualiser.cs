using UnityEngine;

public class ArcVisualizer : MonoBehaviour
{
    [Header("Arc Settings")]
    [Tooltip("Radius of the arc.")]
    public float radius = 50f;
    [Tooltip("Starting angle of the arc in degrees.")]
    public float startAngle = -45f;
    [Tooltip("Ending angle of the arc in degrees.")]
    public float endAngle = 45f;
    [Tooltip("Number of segments to divide the arc.")]
    public int segments = 20;
    [Tooltip("Color of the arc in the Scene view.")]
    public Color arcColor = Color.yellow;

    private void OnDrawGizmos()
    {
        Gizmos.color = arcColor;
        DrawArc();
    }

    private void DrawArc()
    {
        Vector3 center = transform.position;
        float angleStep = (endAngle - startAngle) / segments;
        Vector3 previousPoint = center + Quaternion.Euler(0, -startAngle, 0) * Vector3.forward * radius;

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = startAngle + i * angleStep;
            Vector3 currentPoint = center + Quaternion.Euler(0, -currentAngle, 0) * Vector3.forward * radius;
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }

}
using UnityEngine;

[ExecuteInEditMode]
public class RandomPositionInSector : MonoBehaviour
{
    public float minRadius = 30f; // Minimum distance from the object's position
    public float maxRadius = 50f; // Maximum distance from the object's position
    public float minAngle = -45f; // Minimum angle in degrees
    public float maxAngle = 45f;  // Maximum angle in degrees
    public int segmentCount = 30; // Number of segments to draw the arc
    
    public Vector3 GetRandomPositionInSector(float? fixedAngle = null, float? fixedDistance = null)
    {
        // Determine the radius
        var radius = fixedDistance ?? Random.Range(minRadius, maxRadius);

        // Determine the angle
        var angle = fixedAngle ?? Random.Range(minAngle, maxAngle);

        // Convert angle to radians
        var angleRad = angle * Mathf.Deg2Rad;

        // Calculate local position using polar coordinates
        Vector3 localPosition = new Vector3(
            radius * Mathf.Sin(angleRad), // X coordinate
            0f,                           // Y coordinate (assuming a flat plane)
            radius * Mathf.Cos(angleRad)  // Z coordinate
        );

        // Transform local position to world position
        Vector3 worldPosition = transform.TransformPoint(localPosition);

        return worldPosition;
    }

    void OnDrawGizmosSelected()
    {
        // Save the original Gizmos matrix
        Matrix4x4 originalGizmosMatrix = Gizmos.matrix;

        // Set Gizmos matrix to the object's localToWorldMatrix
        Gizmos.matrix = transform.localToWorldMatrix;

        Gizmos.color = new Color(0, 1, 0, 0.3f); // Semi-transparent green

        // Draw the outer arc
        DrawArc(Vector3.zero, maxRadius, minAngle, maxAngle, segmentCount);

        // Draw the inner arc
        DrawArc(Vector3.zero, minRadius, minAngle, maxAngle, segmentCount);

        // Draw the radial lines
        DrawRadialLine(Vector3.zero, minRadius, maxRadius, minAngle);
        DrawRadialLine(Vector3.zero, minRadius, maxRadius, maxAngle);

        // Restore the original Gizmos matrix
        Gizmos.matrix = originalGizmosMatrix;
    }

    void DrawArc(Vector3 center, float radius, float startAngle, float endAngle, int segments)
    {
        float angleStep = (endAngle - startAngle) / segments;
        Vector3 previousPoint = center + Quaternion.Euler(0, startAngle, 0) * Vector3.forward * radius;

        for (int i = 1; i <= segments; i++)
        {
            float currentAngle = startAngle + i * angleStep;
            Vector3 currentPoint = center + Quaternion.Euler(0, currentAngle, 0) * Vector3.forward * radius;
            Gizmos.DrawLine(previousPoint, currentPoint);
            previousPoint = currentPoint;
        }
    }

    void DrawRadialLine(Vector3 center, float minRadius, float maxRadius, float angle)
    {
        Vector3 direction = Quaternion.Euler(0, angle, 0) * Vector3.forward;
        Vector3 innerPoint = center + direction * minRadius;
        Vector3 outerPoint = center + direction * maxRadius;
        Gizmos.DrawLine(innerPoint, outerPoint);
    }
}

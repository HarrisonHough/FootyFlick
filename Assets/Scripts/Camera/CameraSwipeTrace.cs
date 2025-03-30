using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Camera))]
public class CameraSwipeTrace : MonoBehaviour
{
    [SerializeField] private float pointSpacing = 1f;
    [SerializeField] private LineRenderer lineRenderer;
    private List<Vector2> points = new();
    private Camera targetCamera;

    private void Awake()
    {
        targetCamera = GetComponent<Camera>();
    }

    public void StartTrace(Vector2 startScreenPosition)
    {
        points.Clear();
        lineRenderer.positionCount = 0;

        AddPoint(startScreenPosition);
    }

    public void AddPoint(Vector2 screenPosition)
    {
        if (points.Count == 0 || Vector2.Distance(points[^1], screenPosition) > pointSpacing)
        {
            points.Add(screenPosition);
            lineRenderer.positionCount = points.Count;
            lineRenderer.SetPosition(points.Count - 1, ScreenToWorldPoint(screenPosition));
        }
    }

    public void OnTouchEnd()
    {
        StopAllCoroutines();
        StartCoroutine(ClearAfterDelay(0.5f));
    }

    private void ClearTrace()
    {
        points.Clear();
        lineRenderer.positionCount = 0;
    }

    private Vector3 ScreenToWorldPoint(Vector2 screenPos)
    {
        var worldPos = targetCamera.ScreenToWorldPoint(new Vector3(screenPos.x, screenPos.y, 1f));
        return lineRenderer.transform.InverseTransformPoint(worldPos);
    }
    
    private IEnumerator ClearAfterDelay(float seconds)
    {
        yield return new WaitForSeconds(seconds);
        ClearTrace();
    }
}
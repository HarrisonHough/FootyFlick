using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public struct SwipeData
{
    public Vector2 SwipeVector;
    public float Distance;
    public float Speed;
    public float CurveAmount;     // 0 = straight, higher = more curve
    public float CurveDirection;  // -1 = left, 1 = right (normalized)
}

public class InputHandler : MonoBehaviour
{
    
    [Header("Input Settings")]
    [SerializeField]
    private float minSwipeDistance = 0.05f; // Minimum distance for a swipe (normalized)
    
    [SerializeField]
    private float tapThreshold = 0.2f; // Max duration for tap (seconds)
    
    [SerializeField]
    private float tapMovementThreshold = 0.01f; // Max movement for tap (normalized)
    
    [Header("Swipe Path")]
    [SerializeField] private CameraSwipeTrace swipeTrace;

    private GameInputActions inputActions;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float startTime;
    private float endTime;
    private bool isSwiping = false;
    private bool disableSwipeDetection = false;

    private List<Vector2> swipePath = new();

    public UnityEvent<SwipeData> OnSwipeEvent;
    public UnityEvent<Vector2> OnTapEvent;

    private void Awake()
    {
        inputActions = new GameInputActions();
        inputActions.PlayerControls.TouchPress.started += StartTouch;
        inputActions.PlayerControls.TouchPress.canceled += EndTouch;
    }

    private void OnDestroy()
    {
        if (inputActions == null) return;
        inputActions.PlayerControls.TouchPress.started -= StartTouch;
        inputActions.PlayerControls.TouchPress.canceled -= EndTouch;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    public void SetSwipeDisable(bool value)
    {
        disableSwipeDetection = value;
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        startTouchPosition = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
        swipePath.Clear();
        swipePath.Add(startTouchPosition);
        startTime = (float)context.startTime;
        isSwiping = true;

        swipeTrace.StartTrace(startTouchPosition);
    }

    private void Update()
    {
        if (isSwiping)
        {
            Vector2 current = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
            swipeTrace.AddPoint(current);

            if (swipePath.Count == 0 || Vector2.Distance(current, swipePath[^1]) > 1f)
            {
                swipePath.Add(current);
            }
        }

    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (disableSwipeDetection) return;

        endTouchPosition = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
        endTime = (float)context.time;
        isSwiping = false;

        swipePath.Add(endTouchPosition); // Ensure end is included
        DetectGesture();
        swipeTrace.OnTouchEnd();
    }

    private void DetectGesture()
    {
        var delta = endTouchPosition - startTouchPosition;
        var swipeDistance = delta.magnitude / Screen.height;
        var swipeDuration = endTime - startTime;
        var swipeSpeed = swipeDistance / swipeDuration;

        if (swipeDistance >= minSwipeDistance)
        {
            var (curveAmount, curveDirection) = CalculateCurve(swipePath);

            var swipeData = new SwipeData
            {
                SwipeVector = delta,
                Distance = swipeDistance,
                Speed = swipeSpeed,
                CurveAmount = curveAmount,
                CurveDirection = curveDirection
            };

            OnSwipeEvent.Invoke(swipeData);
        }
        else if (swipeDuration <= tapThreshold && swipeDistance <= tapMovementThreshold)
        {
            OnTapEvent.Invoke(startTouchPosition);
        }
    }

    private (float curveAmount, float curveDirection) CalculateCurve(List<Vector2> path)
    {
        if (path.Count < 3) return (0f, 0f);

        Vector2 start = path[0];
        Vector2 end = path[^1];
        Vector2 lineDir = (end - start).normalized;
        Vector2 lineNormal = new Vector2(-lineDir.y, lineDir.x); // Perpendicular to main swipe

        float totalSignedDeviation = 0f;
        float totalAbsDeviation = 0f;

        for (int i = 1; i < path.Count - 1; i++)
        {
            Vector2 point = path[i];
            Vector2 toPoint = point - start;

            float dot = Vector2.Dot(toPoint, lineDir);
            Vector2 closestPoint = start + lineDir * dot;
            Vector2 deviationVector = point - closestPoint;

            float signedDeviation = Vector2.Dot(deviationVector, lineNormal);
            totalSignedDeviation += signedDeviation;
            totalAbsDeviation += Mathf.Abs(signedDeviation);
        }

        var avgDeviation = totalAbsDeviation / (path.Count - 2);
        var avgSigned = totalSignedDeviation / (path.Count - 2);

        var normalizedCurve = avgDeviation / Screen.height;
        var normalizedDirection = Mathf.Clamp(avgSigned / (Mathf.Abs(avgDeviation) + 0.0001f), -1f, 1f);

        return (normalizedCurve, normalizedDirection);
    }
}

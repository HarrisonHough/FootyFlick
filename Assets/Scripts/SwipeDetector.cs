using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public struct SwipeData
{
    public Vector2 SwipeVector;
    public float Distance;
    public float Speed;    // Speed of the swipe
}

public class SwipeDetector : MonoBehaviour
{
    [SerializeField]
    private float minSwipeDistance = 0.05f;
    private GameInputActions inputActions;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float startTime;
    private float endTime;
    private bool isSwiping = false;
    public UnityEvent<SwipeData> OnSwipeEvent;
    private bool disableSwipeDetection = false;

    private void Awake()
    {
        inputActions = new GameInputActions();

        inputActions.PlayerControls.TouchPress.started += StartTouch;
        inputActions.PlayerControls.TouchPress.canceled += EndTouch;
    }

    private void OnDestroy()
    {
        if(inputActions == null) return;
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
        startTime = (float)context.startTime; // Record the start time
        isSwiping = true;
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (disableSwipeDetection) return;
        endTouchPosition = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
        endTime = (float)context.time; // Record the end time
        isSwiping = false;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        var delta = endTouchPosition - startTouchPosition;
        var swipeDistance = delta.magnitude / Screen.height;
        var swipeDuration = endTime - startTime; 
        var swipeSpeed = swipeDistance / swipeDuration; 

        var swipeData = new SwipeData
        {
            SwipeVector = delta,
            Distance = swipeDistance,
            Speed = swipeSpeed
        };

        if (swipeData.Distance >= minSwipeDistance)
        {
            Debug.Log( $"Swipe detected: {swipeData.SwipeVector}, Distance: {swipeData.Distance}, Speed: {swipeData.Speed}");
            OnSwipeEvent.Invoke(swipeData);
        }
    }
}

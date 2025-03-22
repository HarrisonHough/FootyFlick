using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.InputSystem;

[Serializable]
public struct SwipeData
{
    public Vector2 StartPosition;
    public Vector2 EndPosition;
    public Vector2 Direction;
    public Vector2 SwipeVector;
    public float Distance;
}

public class SwipeDetector : MonoBehaviour
{
    [SerializeField]
    private float minSwipeDistance = 0.1f;
    private GameInputActions inputActions;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private bool isSwiping = false;
    public UnityEvent<SwipeData> OnSwipEvent;
    private bool disableSwipeDetection = false;

    private void Awake()
    {
        inputActions = new GameInputActions();

        inputActions.PlayerControls.TouchPress.started += ctx => StartTouch(ctx);
        inputActions.PlayerControls.TouchPress.canceled += ctx => EndTouch(ctx);
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
        isSwiping = true;
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (disableSwipeDetection) return;
        endTouchPosition = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
        isSwiping = false;
        DetectSwipe();
    }

    private void DetectSwipe()
    {
        var delta = endTouchPosition - startTouchPosition;
        var swipeDistance = delta.magnitude / Screen.height;
        var swipeData = new SwipeData
        {
            StartPosition = startTouchPosition,
            EndPosition = endTouchPosition,
            SwipeVector = delta,
            Direction = delta.normalized,
            Distance = swipeDistance
        };

        if (swipeData.Distance >= minSwipeDistance)
        {
            OnSwipEvent.Invoke(swipeData);
        }
    }
}
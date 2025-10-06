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

public class InputHandler : MonoBehaviour
{
   [SerializeField]
    private float minSwipeDistance = 0.05f; // Minimum distance for a swipe to be registered
    [SerializeField]
    private float tapThreshold = 0.2f; // Maximum duration for a tap to be registered (in seconds)
    [SerializeField]
    private float tapMovementThreshold = 0.01f; // Maximum movement allowed for a tap to be registered
    [SerializeField] private float holdThreshold = 0.35f; // Seconds to trigger hold

    private GameInputActions inputActions;
    private Vector2 startTouchPosition;
    private Vector2 endTouchPosition;
    private float startTime;
    private float endTime;
    private bool isSwiping;
    private bool disableSwipeDetection;
    private bool isHolding;
    private bool holdTriggered;

    public static Action<SwipeData> OnSwipeEvent;
    public static Action<Vector2> OnReverseSwipeEvent; 
    public static Action<Vector2> OnTapEvent;
    public static Action<Vector2> OnHoldStartEvent;
    
    private GameStateEnum gameState;
    
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
    
    private void Update()
    {
        if (isHolding && !holdTriggered)
        {
            float heldTime = Time.time - startTime;
            Vector2 currentTouch = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
            float moveDistance = Vector2.Distance(currentTouch, startTouchPosition) / Screen.height;

            // If movement is small and time exceeds threshold, it's a hold
            if (heldTime >= holdThreshold && moveDistance <= tapMovementThreshold)
            {
                holdTriggered = true;
                isSwiping = false;
                isHolding = false;

                OnHoldStartEvent.Invoke(startTouchPosition);
            }

            // Cancel hold if swipe starts
            if (moveDistance > tapMovementThreshold)
            {
                isHolding = false;
            }
        }
    }

    public void SetSwipeDisabled(bool value)
    {
        disableSwipeDetection = value;
    }

    private void StartTouch(InputAction.CallbackContext context)
    {
        startTouchPosition = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
        startTime = (float)context.startTime;
        isSwiping = true;
        isHolding = true;
        holdTriggered = false;
    }

    private void EndTouch(InputAction.CallbackContext context)
    {
        if (disableSwipeDetection) return;
        endTouchPosition = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
        endTime = (float)context.time;
        isSwiping = false;
        isHolding = false;

        // Skip gesture detection if hold already triggered
        if (!holdTriggered)
        {
            DetectGesture();
        }
    }

    private void DetectGesture()
    {
        
        Vector2 delta = endTouchPosition - startTouchPosition;
        float swipeDistance = delta.magnitude / Screen.height;
        float swipeDuration = endTime - startTime;
        float swipeSpeed = swipeDistance / swipeDuration;

        if (swipeDistance >= minSwipeDistance)
        {
            var angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
            angle = (angle + 360f) % 360f;
            
            if (angle is >= 45f and <= 135f)
            {
                OnSwipeEvent.Invoke(new SwipeData
                {
                    SwipeVector = delta,
                    Distance = swipeDistance,
                    Speed = swipeSpeed
                });
            }
            else
            {
                OnReverseSwipeEvent.Invoke(delta);
            }
        }
        else if (swipeDuration <= tapThreshold && swipeDistance <= tapMovementThreshold)
        {
            OnTapEvent.Invoke(startTouchPosition);
        }
    }
}

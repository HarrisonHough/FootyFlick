using UnityEngine;
using System;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.EnhancedTouch;

public class DragBallLauncher : MonoBehaviour
{
public enum ControlMode { Swipe, Drag }

    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float minForceMultiplier = 0.8f;
    [SerializeField] private float maxForceMultiplier = 1.2f;
    [SerializeField] private GameObject arrowIndicator;

    private float windStrength;
    private Ball ball;
    private Vector2 startTouch, endTouch, currentDrag;
    private bool isDragging;
    private float idealLaunchForce;

    public static Action<float> OnWindChanged;
    [SerializeField]
    private ControlMode currentControlMode = ControlMode.Swipe;

    private GameInputActions inputActions;

    private void Awake()
    {
        inputActions = new GameInputActions();
        inputActions.PlayerControls.TouchPress.started += OnTouchPressStarted;
        inputActions.PlayerControls.TouchPress.canceled += OnTouchPressCanceled;
    }

    private void OnEnable()
    {
        inputActions.Enable();
        EnhancedTouchSupport.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
        EnhancedTouchSupport.Disable();
    }

    private void Start()
    {
        SpawnBall();
        RandomizeWindStrength();
    }

    private void Update()
    {
        if (isDragging && currentControlMode == ControlMode.Drag)
        {
            currentDrag = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>() - startTouch;
            UpdateArrowIndicator();
        }
    }

    private void OnTouchPressStarted(InputAction.CallbackContext context)
    {
        startTouch = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
        if (ball == null) SpawnBall();

        if (currentControlMode == ControlMode.Drag)
        {
            isDragging = true;
            arrowIndicator.SetActive(true);
        }
    }

    private void OnTouchPressCanceled(InputAction.CallbackContext context)
    {
        endTouch = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();

        if (currentControlMode == ControlMode.Swipe)
        {
            idealLaunchForce = CalculateIdealLaunchForce();
            LaunchBall(CalculateSwipeVelocity());
        }
        else if (currentControlMode == ControlMode.Drag && isDragging)
        {
            isDragging = false;
            arrowIndicator.SetActive(false);
            idealLaunchForce = CalculateIdealLaunchForce();
            LaunchBall(CalculateDragVelocity());
        }
    }

    private void LaunchBall(Vector3 velocity)
    {
        if (ball == null) SpawnBall();

        ball.LaunchBall(velocity, KickStyle.DropPunt, cameraTransform);
        ball.SetWindActive(true);
        ball = null;
        RandomizeWindStrength();
    }

    private float CalculateIdealLaunchForce()
    {
        var startPos = transform.position;
        var targetPos = target.position;
        var dx = Vector3.Distance(new Vector3(targetPos.x, 0, targetPos.z), new Vector3(startPos.x, 0, startPos.z));
        var dy = targetPos.y - startPos.y;

        var angleRad = launchAngle * Mathf.Deg2Rad;
        var v = Mathf.Sqrt((gravity * dx * dx) / (2 * Mathf.Cos(angleRad) * Mathf.Cos(angleRad) * (dx * Mathf.Tan(angleRad) - dy)));

        return v;
    }

    private Vector3 CalculateSwipeVelocity()
    {
        var swipeDistance = (endTouch - startTouch).magnitude / Screen.height;
        swipeDistance = Mathf.Clamp(swipeDistance, 0f, 1f);

        var launchForce = Mathf.Lerp(idealLaunchForce * minForceMultiplier, idealLaunchForce * maxForceMultiplier, swipeDistance);

        var swipeDirection = (endTouch - startTouch).normalized;
        var direction = new Vector3(swipeDirection.x, 0, swipeDirection.y).normalized;

        var angleRad = launchAngle * Mathf.Deg2Rad;
        var velocity = direction * launchForce * Mathf.Cos(angleRad);
        velocity.y = launchForce * Mathf.Sin(angleRad);

        return velocity;
    }

    private Vector3 CalculateDragVelocity()
    {
        if (currentDrag.magnitude < 10f) return Vector3.zero;

        var dragDistance = currentDrag.magnitude / Screen.height;
        dragDistance = Mathf.Clamp(dragDistance, 0f, 1f);

        var launchForce = Mathf.Lerp(idealLaunchForce * minForceMultiplier, idealLaunchForce * maxForceMultiplier, dragDistance);

        var horizontalDirection = new Vector3(-currentDrag.normalized.x, 0, -currentDrag.normalized.y).normalized;

        var angleRad = launchAngle * Mathf.Deg2Rad;
        var velocity = horizontalDirection * launchForce * Mathf.Cos(angleRad);
        velocity.y = launchForce * Mathf.Sin(angleRad);

        return velocity;
    }

    private void UpdateArrowIndicator()
    {
        var dragDirection = -currentDrag.normalized;
        var dragMagnitude = Mathf.Clamp(currentDrag.magnitude / Screen.height, 0f, 1f);

        var direction3D = new Vector3(dragDirection.x, 0, dragDirection.y).normalized;

        if (ball == null) SpawnBall();
        arrowIndicator.transform.rotation = Quaternion.LookRotation(direction3D);
        var scale = dragMagnitude * 3;
        arrowIndicator.transform.localScale = new Vector3(scale, scale, scale);
    }

    private void RandomizeWindStrength()
    {
        windStrength = UnityEngine.Random.Range(-6f, 6f);
        OnWindChanged?.Invoke(windStrength);
    }

    private void SpawnBall()
    {
        ball = Instantiate(ballPrefab);
        ball.transform.position = transform.position;
    }
}

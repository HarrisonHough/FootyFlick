using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float maxLaunchForce = 10f;
    [SerializeField] private float maxSwipeDistance = 0.5f; // Proportion of screen height
    [SerializeField] private GameObject arrowIndicator;
    [SerializeField] private float windStrengthRange = 6f; // Maximum wind strength
    [SerializeField] private float windChangeInterval = 5f; // Time in seconds between wind changes

    private Ball ball;
    private Vector2 startTouch, endTouch, currentDrag;
    private bool isDragging;
    private float windStrength;
    private Vector3 windDirection;
    private GameInputActions inputActions;
    private float windTimer;

    public static Action<float> OnWindChanged;

    private void Awake()
    {
        inputActions = new GameInputActions();
        inputActions.PlayerControls.TouchPress.started += OnTouchPressStarted;
        inputActions.PlayerControls.TouchPress.canceled += OnTouchPressCanceled;
    }

    private void OnEnable()
    {
        inputActions.Enable();
    }

    private void OnDisable()
    {
        inputActions.Disable();
    }

    private void Start()
    {
        SpawnBall();
        RandomizeWindStrength();
    }

    private void Update()
    {
        if (isDragging)
        {
            currentDrag = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>() - startTouch;
            UpdateArrowIndicator();
        }

        // Update wind strength at intervals
        windTimer += Time.deltaTime;
        if (windTimer >= windChangeInterval)
        {
            RandomizeWindStrength();
            windTimer = 0f;
        }
    }

    private void OnTouchPressStarted(InputAction.CallbackContext context)
    {
        startTouch = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();
        if (ball == null) SpawnBall();

        isDragging = true;
        arrowIndicator.SetActive(true);
    }

    private void OnTouchPressCanceled(InputAction.CallbackContext context)
    {
        endTouch = inputActions.PlayerControls.TouchPosition.ReadValue<Vector2>();

        if (isDragging)
        {
            isDragging = false;
            arrowIndicator.SetActive(false);
            Vector3 launchVelocity = CalculateLaunchVelocity();

            // Launch the ball only if the velocity is non-zero
            if (launchVelocity != Vector3.zero)
            {
                LaunchBall(launchVelocity);
            }
        }
    }

    private Vector3 CalculateLaunchVelocity()
    {
        Vector2 swipeVector = endTouch - startTouch;

        // Ignore swipes with an upward or neutral vertical component
        if (swipeVector.y >= 0)
        {
            return Vector3.zero;
        }

        // Calculate the swipe distance as a proportion of the screen height
        float swipeDistance = Mathf.Clamp(swipeVector.magnitude / Screen.height, 0f, maxSwipeDistance);

        // Determine the launch force based on the swipe distance
        float launchForce = swipeDistance / maxSwipeDistance * maxLaunchForce;

        // Calculate the launch direction
        Vector3 launchDirection = new Vector3(-swipeVector.x, 0, -swipeVector.y).normalized;

        // Apply the launch angle to determine the velocity components
        float angleRad = launchAngle * Mathf.Deg2Rad;
        Vector3 velocity = launchDirection * launchForce * Mathf.Cos(angleRad);
        velocity.y = launchForce * Mathf.Sin(angleRad);

        return velocity;
    }
    
    private void UpdateArrowIndicator()
    {
        Vector2 dragDirection = currentDrag.normalized;

        // Check if the swipe is backward (upward swipe in screen space)
        if (dragDirection.y > 0)
        {
            // Hide the arrow indicator for backward swipes
            arrowIndicator.SetActive(false);
            return;
        }

        // Show the arrow indicator for valid forward swipes
        arrowIndicator.SetActive(true);

        float dragMagnitude = Mathf.Clamp(currentDrag.magnitude / (Screen.height * maxSwipeDistance), 0f, 1f);

        // Map downward drag to forward direction
        Vector3 direction3D = new Vector3(-dragDirection.x, 0, -dragDirection.y).normalized;

        arrowIndicator.transform.rotation = Quaternion.LookRotation(direction3D);
        float scale = dragMagnitude * 3; // Adjust the multiplier as needed
        arrowIndicator.transform.localScale = new Vector3(scale, scale, scale);
    }
    
    private void LaunchBall(Vector3 velocity)
    {
        if (ball == null) SpawnBall();
        ball.SetWind(windStrength, cameraTransform.right);
        ball.SetWindActive(true);
        ball.LaunchBall(velocity);
        ball = null;
    }

    private void RandomizeWindStrength()
    {
        windStrength = UnityEngine.Random.Range(-windStrengthRange, windStrengthRange);
        windDirection.Normalize();

        OnWindChanged?.Invoke(windStrength);
    }

    private void SpawnBall()
    {
        ball = Instantiate(ballPrefab);
        ball.transform.position = transform.position;
    }
}

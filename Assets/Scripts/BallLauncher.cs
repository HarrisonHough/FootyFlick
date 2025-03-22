using System;
using UnityEngine;
using UnityEngine.InputSystem;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform ballParent;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float maxLaunchForce = 10f;
    [SerializeField] private float maxSwipeDistance = 0.5f; // Proportion of screen height
    [SerializeField] private float windStrengthRange = 6f; // Maximum wind strength
    [SerializeField] private float windChangeInterval = 5f; // Time in seconds between wind changes

    private Ball ball;
    private float windStrength;
    private GameInputActions inputActions;
    private float windTimer;

    public static Action<float> OnWindChanged;

    private void Awake()
    {
        inputActions = new GameInputActions();
        inputActions.PlayerControls.TouchPress.started += OnTouchPressStarted;
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
        // Update wind strength at intervals
        windTimer += Time.deltaTime;
        if (windTimer >= windChangeInterval)
        {
            RandomizeWindStrength();
            windTimer = 0f;
        }
    }
    
    public void OnSwipeDetected(SwipeData swipeData)
    {
        if (ball == null) SpawnBall();
        Vector3 launchVelocity = CalculateLaunchVelocity(swipeData);
        LaunchBall(launchVelocity);
    }

    private void OnTouchPressStarted(InputAction.CallbackContext context)
    {
        if (ball == null) SpawnBall();
    }

    private Vector3 CalculateLaunchVelocity(SwipeData swipeData)
    {
        // Ignore swipes with an upward or neutral vertical component
        if (swipeData.SwipeVector.y < 0)
        {
            return Vector3.zero;
        }

        // Calculate the swipe distance as a proportion of the screen height
        var clampedDistance = Mathf.Clamp(swipeData.Distance, 0f, maxSwipeDistance);

        // Determine the launch force based on the swipe distance
        var launchForce = clampedDistance / maxSwipeDistance * maxLaunchForce;

        // Calculate the launch direction
        var launchDirection = new Vector3(swipeData.SwipeVector.x, 0, swipeData.SwipeVector.y).normalized;

        // Apply the launch angle to determine the velocity components
        var angleRad = launchAngle * Mathf.Deg2Rad;
        var velocity = launchDirection * launchForce * Mathf.Cos(angleRad);
        velocity.y = launchForce * Mathf.Sin(angleRad);

        return velocity;
    }
    
    private void LaunchBall(Vector3 velocity)
    {
        if (ball == null) SpawnBall();
        ball.SetWind(windStrength, cameraTransform.right.normalized);
        ball.SetWindActive(true);
        ball.LaunchBall(velocity);
        ball = null;
    }

    private void RandomizeWindStrength()
    {
        windStrength = UnityEngine.Random.Range(-windStrengthRange, windStrengthRange);
        OnWindChanged?.Invoke(windStrength);
    }

    private void SpawnBall()
    {
        ball = Instantiate(ballPrefab);
        ball.transform.position = ballParent.transform.position;
    }
}

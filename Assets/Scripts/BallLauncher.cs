using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallLauncher : MonoBehaviour
{
    public enum ControlMode { Swipe, Drag }

    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform target;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float gravity = 9.8f;
    [SerializeField] private float minForceMultiplier = 0.8f; // Adjust to make shots possible but challenging
    [SerializeField] private float maxForceMultiplier = 1.2f; // Allows overshooting
    [SerializeField] private GameObject arrowIndicator;

    private float windStrength;
    private Ball ball;
    private Vector2 startTouch, endTouch, currentDrag;
    private bool isDragging;
    private float idealLaunchForce;

    public static Action<float> OnWindChanged;
    [SerializeField]
    private ControlMode currentControlMode = ControlMode.Swipe;

    private void Start()
    {
        SpawnBall();
        RandomizeWindStrength();
    }

    private void Update()
    {
        if (currentControlMode == ControlMode.Swipe)
        {
            HandleSwipeInput();
        }
        else if (currentControlMode == ControlMode.Drag)
        {
            HandleDragInput();
        }
    }

    private void HandleSwipeInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            if (ball == null) SpawnBall();
        }

        if (Input.GetMouseButtonUp(0))
        {
            endTouch = Input.mousePosition;
            idealLaunchForce = CalculateIdealLaunchForce(); // Update force range before launching
            LaunchBall(CalculateSwipeVelocity());
        }
    }

    private void HandleDragInput()
    {
        if (Input.GetMouseButtonDown(0))
        {
            startTouch = Input.mousePosition;
            isDragging = true;
            arrowIndicator.SetActive(true);
        }

        if (Input.GetMouseButton(0) && isDragging)
        {
            currentDrag = (Vector2)Input.mousePosition - startTouch;
            UpdateArrowIndicator();
        }

        if (Input.GetMouseButtonUp(0) && isDragging)
        {
            isDragging = false;
            arrowIndicator.SetActive(false);
            idealLaunchForce = CalculateIdealLaunchForce(); // Update force range before launching
            LaunchBall(CalculateDragVelocity());
        }
    }

    private void LaunchBall(Vector3 velocity)
    {
        if (ball == null) SpawnBall();

        ball.LaunchBall(velocity);
        ball.SetWind(windStrength, cameraTransform.right);
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

        return v; // This is the "ideal" force required to reach the target
    }

    private Vector3 CalculateSwipeVelocity()
    {
        // Get swipe distance as a percentage of screen height
        var swipeDistance = (endTouch - startTouch).magnitude / Screen.height;
        swipeDistance = Mathf.Clamp(swipeDistance, 0f, 1f); // Ensure it's within valid range

        // Scale launch force within adjusted range (based on target distance)
        var launchForce = Mathf.Lerp(idealLaunchForce * minForceMultiplier, idealLaunchForce * maxForceMultiplier, swipeDistance);

        // Get swipe direction & normalize to 3D
        var swipeDirection = (endTouch - startTouch).normalized;
        var direction = new Vector3(swipeDirection.x, 0, swipeDirection.y).normalized;

        // Apply launch angle for proper arc trajectory
        var angleRad = launchAngle * Mathf.Deg2Rad;
        var velocity = direction * launchForce * Mathf.Cos(angleRad); // Horizontal component
        velocity.y = launchForce * Mathf.Sin(angleRad); // Vertical component

        return velocity;
    }
    
    private Vector3 CalculateDragVelocity()
    {
        if (currentDrag.magnitude < 10f) return Vector3.zero; // Ignore tiny drags

        // Get drag distance as a percentage of screen height
        var dragDistance = currentDrag.magnitude / Screen.height;
        dragDistance = Mathf.Clamp(dragDistance, 0f, 1f);

        // Scale launch force within adjusted range
        var launchForce = Mathf.Lerp(idealLaunchForce * minForceMultiplier, idealLaunchForce * maxForceMultiplier, dragDistance);

        // Convert 2D drag direction to 3D
        var horizontalDirection = new Vector3(-currentDrag.normalized.x, 0, -currentDrag.normalized.y).normalized;

        // Apply launch angle for arc trajectory
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
        windStrength = Random.Range(-6f, 6f);
        OnWindChanged?.Invoke(windStrength);
    }

    private void SpawnBall()
    {
        ball = Instantiate(ballPrefab);
        ball.transform.position = transform.position;
    }
}

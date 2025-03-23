using System;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Transform ballParent;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float maxLaunchForce = 10f;
    [SerializeField] private float maxSwipeDistance = 0.5f; // Proportion of screen height
    [SerializeField] private Pool ballPool;

    private Ball currentBall;
    public static Action OnBallLaunched;
    private void Start()
    {
        GameController.OnKickReady += OnKickReady;
    }

    private void OnDestroy()
    {
        GameController.OnKickReady -= OnKickReady;
    }

    private void OnKickReady()
    {
        if (currentBall == null) SpawnBall();
    }

    public void OnSwipeDetected(SwipeData swipeData)
    {
        if (currentBall == null) return;
        Vector3 launchVelocity = CalculateLaunchVelocity(swipeData);
        LaunchBall(launchVelocity);
    }

    private Vector3 CalculateLaunchVelocity(SwipeData swipeData)
    {
        // Ignore swipes with a downward or neutral vertical component
        if (swipeData.SwipeVector.y <= 0)
        {
            return Vector3.zero;
        }

        // Calculate the swipe distance as a proportion of the screen height
        var clampedDistance = Mathf.Clamp(swipeData.Distance, 0f, maxSwipeDistance);

        // Determine the launch force based on the swipe distance and speed
        var distanceFactor = clampedDistance / maxSwipeDistance;
        var speedFactor = Mathf.Clamp(swipeData.Speed, 0f, 1f); // Assuming swipe speed is normalized between 0 and 1
        var launchForce = (distanceFactor + speedFactor) * 0.5f * maxLaunchForce;

        // Get the camera's forward direction, ignoring the y component
        var cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        // Get the camera's right direction
        var cameraRight = cameraTransform.right;

        // Calculate the launch direction relative to the camera's orientation
        var launchDirection = (cameraRight * swipeData.SwipeVector.x + cameraForward * swipeData.SwipeVector.y).normalized;

        // Apply the launch angle to determine the velocity components
        var angleRad = launchAngle * Mathf.Deg2Rad;
        var velocity = launchDirection * launchForce * Mathf.Cos(angleRad);
        velocity.y = launchForce * Mathf.Sin(angleRad);

        return velocity;
    }

    private void LaunchBall(Vector3 velocity)
    {
        if (currentBall == null) SpawnBall();
        currentBall.transform.parent = null;
        currentBall.LaunchBall(velocity);
        currentBall = null;
        OnBallLaunched?.Invoke();
    }

    private void SpawnBall()
    {
        GameObject ballObject = ballPool.GetObject(ballParent.position, ballParent.rotation);
        ballObject.transform.parent = ballParent;
        currentBall = ballObject.GetComponent<Ball>();
    }
}

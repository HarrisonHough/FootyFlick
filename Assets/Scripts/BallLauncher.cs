using System;
using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Ball ballPrefab;
    [SerializeField] private Transform ballParent;
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float maxLaunchForce = 10f;
    [SerializeField] private float maxSwipeDistance = 0.5f; // Proportion of screen height
    
    private Ball ball;
    
    private void Start()
    {
        SpawnBall();
        Ball.OnBallScoreComplete += OnBallScoreComplete;
    }

    private void OnBallScoreComplete(BallScoreData obj)
    {
        if (ball == null) SpawnBall();
    }

    public void OnSwipeDetected(SwipeData swipeData)
    {
        if (ball == null) return;
        Vector3 launchVelocity = CalculateLaunchVelocity(swipeData);
        LaunchBall(launchVelocity);
    }

    private Vector3 CalculateLaunchVelocity(SwipeData swipeData)
    {
        // Ignore swipes with an upward or neutral vertical component
        if (swipeData.SwipeVector.y < 0)
        {
            return Vector3.zero;
        }

        // Calculate the swipe distance as a proportion of the screen height
        float clampedDistance = Mathf.Clamp(swipeData.Distance, 0f, maxSwipeDistance);

        // Determine the launch force based on the swipe distance
        float launchForce = clampedDistance / maxSwipeDistance * maxLaunchForce;

        // Get the camera's forward direction, ignoring the y component
        Vector3 cameraForward = cameraTransform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        // Get the camera's right direction
        Vector3 cameraRight = cameraTransform.right;

        // Calculate the launch direction relative to the camera's orientation
        Vector3 launchDirection = (cameraRight * swipeData.SwipeVector.x + cameraForward * swipeData.SwipeVector.y).normalized;

        // Apply the launch angle to determine the velocity components
        float angleRad = launchAngle * Mathf.Deg2Rad;
        Vector3 velocity = launchDirection * launchForce * Mathf.Cos(angleRad);
        velocity.y = launchForce * Mathf.Sin(angleRad);

        return velocity;
    }
    
    private void LaunchBall(Vector3 velocity)
    {
        if (ball == null) SpawnBall();
        ball.transform.parent = null;
        ball.LaunchBall(velocity);
        ball = null;
    }

    private void SpawnBall()
    {
        ball = Instantiate(ballPrefab, ballParent.transform);
        ball.transform.position = ballParent.position;
    }
}

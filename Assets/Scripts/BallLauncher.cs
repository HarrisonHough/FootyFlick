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
    [SerializeField] private float windStrengthRange = 6f; // Maximum wind strength

    private Ball ball;
    private float windStrength;
    private float windTimer;

    //public static Action<float> OnWindChanged;
    
    private void Start()
    {
        SpawnBall();
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
        ball. SetWind(windStrength, cameraTransform.right.normalized);
        ball.SetWindActive(true);
        ball.LaunchBall(velocity);
        ball = null;
    }

    // private void RandomizeWindStrength()
    // {
    //     windStrength = UnityEngine.Random.Range(-windStrengthRange, windStrengthRange);
    //     OnWindChanged?.Invoke(windStrength);
    // }

    private void SpawnBall()
    {
        ball = Instantiate(ballPrefab);
        ball.transform.position = ballParent.transform.position;
    }
}

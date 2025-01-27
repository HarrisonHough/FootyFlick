using System;
using UnityEngine;
using Random = UnityEngine.Random;

public class BallLauncher : MonoBehaviour
{
    [SerializeField]
    private Ball ballPrefab;
    [SerializeField]
    private Transform target;
    [SerializeField]
    private Transform cameraTransform;
    [SerializeField]
    private float launchAngle = 45f; // Launch angle in degrees
    [SerializeField]
    private float gravity = 9.8f; // Gravitational acceleration
    private float windStrength;
    private Vector2 startTouch, endTouch;
    private Ball ball;

    public static Action<float> OnWindChanged;

    private void Start()
    {
        SpawnBall();
        RandomizeWindStrength();
    }
    
    private void RandomizeWindStrength()
    {
        windStrength = Random.Range(-10f, 10f);
        OnWindChanged?.Invoke(windStrength);
    }

    private void SpawnBall()
    {
        ball = Instantiate(ballPrefab);
        ball.transform.position = transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Swipe start
        {
            startTouch = Input.mousePosition;
            if(ball == null)
            {
                SpawnBall();
            }
        }

        if (Input.GetMouseButtonUp(0)) // Swipe end
        {
            endTouch = Input.mousePosition;
            LaunchBall();
        }
    }

    private void LaunchBall()
    {
        ball.LaunchBall( CalculateLaunchVelocity() );
        ball.SetWind(windStrength, cameraTransform.right);
        ball.SetWindActive(true);
        ball = null;
        RandomizeWindStrength();
    }

    private Vector3 CalculateLaunchVelocity()
    {

        // Calculate swipe direction
        var swipeDirection = (endTouch - startTouch).normalized;
        var direction = new Vector3(swipeDirection.x, 0, swipeDirection.y).normalized;

        // Calculate force magnitude (ignoring direction for this step)
        var startPos = ball.transform.position;
        var targetPos = target.position;

        var dx = Vector3.Distance(new Vector3(targetPos.x, 0, targetPos.z), new Vector3(startPos.x, 0, startPos.z));
        var dy = targetPos.y - startPos.y;

        var angleRad = launchAngle * Mathf.Deg2Rad;

        var cosAngle = Mathf.Cos(angleRad);
        var sinAngle = Mathf.Sin(angleRad);
        var v = Mathf.Sqrt((gravity * dx * dx) / (2 * cosAngle * cosAngle * (dx * Mathf.Tan(angleRad) - dy)));

        // Combine direction with force
        var velocity = direction * v * cosAngle; // Horizontal component
        velocity.y = v * sinAngle; // Vertical component
        return velocity;
    }
}

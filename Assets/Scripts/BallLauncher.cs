using UnityEngine;

public class BallLauncher : MonoBehaviour
{
    public Transform ball;       // Ball Transform
    public Transform target;     // Target Transform
    public float launchAngle = 45f; // Launch angle in degrees
    public float gravity = 9.8f; // Gravitational acceleration
    public float windStrength = 5f; // Wind strength (positive = right, negative = left)

    private Vector2 startTouch, endTouch;

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) // Swipe start
        {
            startTouch = Input.mousePosition;
        }

        if (Input.GetMouseButtonUp(0)) // Swipe end
        {
            endTouch = Input.mousePosition;
            LaunchBall();
        }
    }

    void LaunchBall()
    {
        // Calculate swipe direction
        Vector2 swipeDirection = (endTouch - startTouch).normalized;
        Vector3 direction = new Vector3(swipeDirection.x, 0, swipeDirection.y).normalized;

        // Calculate force magnitude (ignoring direction for this step)
        Vector3 startPos = ball.position;
        Vector3 targetPos = target.position;

        float dx = Vector3.Distance(new Vector3(targetPos.x, 0, targetPos.z), new Vector3(startPos.x, 0, startPos.z));
        float dy = targetPos.y - startPos.y;

        float angleRad = launchAngle * Mathf.Deg2Rad;

        float cosAngle = Mathf.Cos(angleRad);
        float sinAngle = Mathf.Sin(angleRad);
        float v = Mathf.Sqrt((gravity * dx * dx) / (2 * cosAngle * cosAngle * (dx * Mathf.Tan(angleRad) - dy)));

        // Combine direction with force
        Vector3 velocity = direction * v * cosAngle; // Horizontal component
        velocity.y = v * sinAngle; // Vertical component

        // Apply velocity to the Rigidbody
        Rigidbody rb = ball.GetComponent<Rigidbody>();
        rb.linearVelocity = velocity;
    }

    void FixedUpdate()
    {
        // Apply wind force continuously
        Vector3 windForce = new Vector3(windStrength, 0, 0); // Wind affects horizontal direction
        ball.GetComponent<Rigidbody>().AddForce(windForce, ForceMode.Acceleration);
    }
}

using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody ballRigidbody; 
    [SerializeField]
    private float windStrength = 5f; // Wind strength (positive = right, negative = left)
    [SerializeField]
    private float spinSpeed = 10f; 
    private Vector3 windDirection = new Vector3(1,0,0);
    
    private bool windActive;
    
    private void Start()
    {
        if(ballRigidbody == null)
        {
            ballRigidbody = GetComponent<Rigidbody>();
        }
        DeactivateRigidbody();
    }
    

    
    public void SetWindActive(bool active)
    {
        windActive = active;
    }
    
    public void SetWind(float strength, Vector3 direction)
    {
        windStrength = strength;
        windDirection = direction;
    }
    
    public void LaunchBall(Vector3 velocity)
    {
        ActivateRigidbody();
        ballRigidbody.linearVelocity = velocity;
        ballRigidbody.angularVelocity = transform.right * spinSpeed; // Backward spin
    }

    private void FixedUpdate()
    {
        if(windActive)
        {
            ApplyWindForce();
        }
    }

    private void ApplyWindForce()
    {
        var windForce = windDirection * windStrength;
        ballRigidbody.AddForce(windForce, ForceMode.Acceleration);
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        //TODO: check if we need to use tags or layers to identify certain objects
        SetWindActive(false);
    }
    
    public void DeactivateRigidbody()
    {
        ballRigidbody.useGravity = false;
        ballRigidbody.linearVelocity = Vector3.zero;
        ballRigidbody.angularVelocity = Vector3.zero;
        ballRigidbody.isKinematic = true; 
    }
    
    public void ActivateRigidbody()
    {
        ballRigidbody.useGravity = true;
        ballRigidbody.isKinematic = false; 
    }
}

using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField]
    private Transform goalTarget;
    
    private void Start()
    {

    }
    
    public void MoveToPosition(Vector3 position)
    {
        // Update the player's position
        transform.position = position;

        // Calculate the direction to the target, ignoring the Y-axis
        Vector3 directionToTarget = goalTarget.position - transform.position;
        directionToTarget.y = 0; // Ignore vertical differences

        // Check if the direction is not zero to avoid errors
        if (directionToTarget.sqrMagnitude > 0.001f)
        {
            // Calculate the rotation needed to face the target
            Quaternion targetRotation = Quaternion.LookRotation(directionToTarget);

            // Apply the rotation to the player
            transform.rotation = targetRotation;
        }
        
    }
}

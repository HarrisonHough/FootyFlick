using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

[Serializable]
public enum KickStyle
{
    DropPunt,
    SnapLeft,
    SnapRight
}

public class BallLauncher : MonoBehaviour
{
    [SerializeField] private Transform ballParent;
    [SerializeField] private Camera camera;
    [SerializeField] private float launchAngle = 45f;
    [SerializeField] private float maxLaunchForce = 10f;
    [SerializeField] private float maxSwipeDistance = 0.5f; // Proportion of screen height
    [SerializeField] private Pool ballPool;
    [SerializeField] private float minVerticalSwipe = 0.1f; // Minimum vertical swipe distance to be considered a valid swipe
    
    private Ball currentBall;
    public static Action OnBallLaunched;
    private Quaternion targetRotation;
    private KickStyle currentKickStyle;
    public UnityEvent OnBallTap;

    private static readonly Dictionary<KickStyle, Vector3> kickStyleRotations = new Dictionary<KickStyle, Vector3>
    {
        { KickStyle.DropPunt, new Vector3(64, 0, 0) },
        { KickStyle.SnapLeft, new Vector3(33, 130, 0) },
        { KickStyle.SnapRight, new Vector3(33, -130, 0) }
    };

    private void Start()
    {
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateEnum gameState)
    {
        if (gameState != GameStateEnum.GameKicking) return; 
        OnKickReady();
        
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnKickReady()
    {
        if (currentBall == null) SpawnBall();
    }
    
    public void OnTap(Vector2 screenPosition)
    {
        if (IsPointerOverUIObject())
            return; 

        Ray ray = camera.ScreenPointToRay(screenPosition);
        if (Physics.Raycast(ray, out RaycastHit hit))
        {
            if (hit.collider.CompareTag("Ball"))
            {
                // Ball was tapped or clicked
                OnBallTapped();
            }
        }
    }
    
    private bool IsPointerOverUIObject()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = Pointer.current.position.ReadValue()
        };
        var results = new System.Collections.Generic.List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        return results.Count > 0;
    }
    
    private void OnBallTapped()
    {
        Debug.Log("Ball was tapped!");
        
        OnBallTap?.Invoke();

        return;
        
        KickStyle[] styles = (KickStyle[])Enum.GetValues(typeof(KickStyle));

        // Find the index of the current style
        int currentIndex = Array.IndexOf(styles, currentKickStyle);

        // Calculate the next index, wrapping around if necessary
        int nextIndex = (currentIndex + 1) % styles.Length;

        // Update the current kick style
        currentKickStyle = styles[nextIndex];
        StopAllCoroutines();
        StartCoroutine(RotateOverTime(kickStyleRotations[currentKickStyle], 0.2f));
    }
    
    public void SetKickStyle(KickStyle kickStyle)
    {
        if(currentKickStyle == kickStyle)
        {
            return;
        }
        currentKickStyle = kickStyle;
        StopAllCoroutines();
        StartCoroutine(RotateOverTime(kickStyleRotations[kickStyle], 0.2f));
    }

    public void OnSwipeDetected(SwipeData swipeData)
    {
        if (currentBall == null) return;

        if (swipeData.Distance > minVerticalSwipe)
        {
            LaunchBall(swipeData);
            return;
        }
    }

    public void HandleReverseSwipe(Vector2 delta)
    {
        float angle = Mathf.Atan2(delta.y, delta.x) * Mathf.Rad2Deg;
        angle = (angle + 360f) % 360f;

        Debug.Log($"Reverse swipe angle: {angle}");

        if (angle >= 135f && angle < 250f)
        {
            SetKickStyle(KickStyle.SnapLeft); // Down-left
        }
        else if (angle >= 250f && angle < 290f)
        {
            SetKickStyle(KickStyle.DropPunt); // Straight down (40° range)
        }
        else if (angle >= 290f || angle < 45f)
        {
            SetKickStyle(KickStyle.SnapRight); // Down-right
        }
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
        var cameraForward = camera.gameObject.transform.forward;
        cameraForward.y = 0;
        cameraForward.Normalize();

        // Get the camera's right direction
        var cameraRight = camera.gameObject.transform.right;

        // Calculate the launch direction relative to the camera's orientation
        var launchDirection = (cameraRight * swipeData.SwipeVector.x + cameraForward * swipeData.SwipeVector.y).normalized;

        // Apply the launch angle to determine the velocity components
        var angleRad = launchAngle * Mathf.Deg2Rad;
        var velocity = launchDirection * launchForce * Mathf.Cos(angleRad);
        velocity.y = launchForce * Mathf.Sin(angleRad);

        return velocity;
    }

    private void LaunchBall(SwipeData swipeData)
    {
        var launchVelocity = CalculateLaunchVelocity(swipeData);
        if (currentBall == null) SpawnBall();
        if (launchVelocity.magnitude < 0.05f)
        {
            return;
        }
        GameManager.SetGameState(GameStateEnum.GameKicked);
        currentBall.transform.parent = null;
        currentBall.LaunchBall(launchVelocity, currentKickStyle, camera.transform);
        currentBall = null;
        OnBallLaunched?.Invoke();
    }

    private void SpawnBall()
    {
        GameObject ballObject = ballPool.GetObject(ballParent.position, ballParent.rotation);
        ballObject.transform.parent = ballParent;
        currentBall = ballObject.GetComponent<Ball>();
        currentKickStyle = KickStyle.DropPunt;
    }

    public IEnumerator RotateOverTime(Vector3 targetLocalRotation, float duration)
    {
        // Convert target local Euler angles to a quaternion
        var localRotation = Quaternion.Euler(targetLocalRotation);

        // Calculate the target world rotation relative to the parent's rotation
        var targetWorldRotation = transform.rotation * localRotation;

        // Store the initial rotation of the currentBall
        var initialRotation = currentBall.transform.rotation;

        var elapsedTime = 0f;

        // Perform the rotation over the specified duration
        while (elapsedTime < duration)
        {
            currentBall.transform.rotation = Quaternion.Slerp(initialRotation, targetWorldRotation, elapsedTime / duration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        // Ensure the final rotation is set precisely
        currentBall.transform.rotation = targetWorldRotation;
    }
    
}

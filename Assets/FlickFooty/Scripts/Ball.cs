using System;
using System.Collections;
using UnityEngine;

public class Ball : MonoBehaviour
{
    [SerializeField]
    private Rigidbody ballRigidbody; 
    [SerializeField]
    private float windStrength = 5f; // Wind strength (positive = right, negative = left)
    [SerializeField]
    private float spinSpeed = 10f; 
    [SerializeField]
    private float minBallSpeed = 0.2f;
    [SerializeField]
    private float maxBallLifeTime = 3f;
    [SerializeField]
    private float magnusCoefficient = 0.1f; // Adjust this value to fine-tune the Magnus effect
    [SerializeField]
    private float curvingForceMagnitude = 5f;
    private Vector3 curvingForceDirection = Vector3.zero;
    private Vector3 windDirection = new Vector3(1,0,0);
    private bool windActive;
    
    private bool disableScoring;
    

    private KickData currentKickData;
    public static Action<KickData> OnKickComplete;
    private PoolMember poolMember;
    private MeshRenderer ballRenderer;
    
    private void Start()
    {
        disableScoring = true;
        GameManager.OnGameStateChanged += OnGameStateChanged;
        ballRenderer = GetComponent<MeshRenderer>();
    }

    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }

    private void OnEnable()
    {
        currentKickData.Result = KickResult.None;
        if(ballRigidbody == null)
        {
            ballRigidbody = GetComponent<Rigidbody>();
        }
        DeactivateRigidbody();
    }

    private void OnDisable()
    {
        StopAllCoroutines();
    }
    
    public void OnGameStateChanged(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.GameOver:
                Reset();
                break;
            default:
                break;
        }
    }

    private void Reset()
    {
        SetWindActive(false);
        StopAllCoroutines();
        if(poolMember != null)
        {
            poolMember.ReturnToPool(0);
        }
        
    }
    
    public void SetWindActive(bool active)
    {
        windActive = active;
    }

    public void LaunchBall(Vector3 velocity, KickStyle kickStyle, Transform cameraTransform)
    {
        if (poolMember == null)
        {
            poolMember = GetComponent<PoolMember>();
        }
        disableScoring = false;
        ActivateRigidbody();
        ballRigidbody.linearVelocity = velocity;
        Vector3 cameraRight = cameraTransform.right;
        cameraRight.y = 0;
        cameraRight.Normalize();
        
        currentKickData.Style = kickStyle;
        // Set angular velocity based on kick style
        switch (currentKickData.Style)
        {
            default:
                curvingForceDirection = Vector3.zero;
                ballRigidbody.angularVelocity = transform.right * spinSpeed; // Backward spin
                break;
            case KickStyle.SnapLeft:
                curvingForceDirection = -cameraRight;
                ballRigidbody.angularVelocity = transform.right * spinSpeed * 2; // Spin around the up axis
                break;
            case KickStyle.SnapRight:
                curvingForceDirection = cameraRight;
                ballRigidbody.angularVelocity = transform.right * spinSpeed * 2; // Spin around the up axis in the opposite direction
                break;
        }

        SetWindActive(true);
        windStrength = WindControl.Instance.WindForce;
        windDirection = WindControl.Instance.WindDirection;
        StartCoroutine(WaitForBallToStop());
    }

    private void FixedUpdate()
    {
        if (windActive)
        {
            ApplyWindForce();
        }

        if (currentKickData.Style != KickStyle.DropPunt)
        {
            ApplyCurvingForce();
        }

        if (ballRenderer == null)
        {
            ballRenderer = GetComponent<MeshRenderer>();
        }

        if (!ballRenderer.isVisible && !disableScoring )
        {
            BallScored();
        }
    }

    private void ApplyCurvingForce()
    {
        ballRigidbody.AddForce(curvingForceDirection.normalized * curvingForceMagnitude, ForceMode.Force);
    }

    private void ApplyWindForce()
    {
        var windForce = windDirection * windStrength;
        ballRigidbody.AddForce(windForce, ForceMode.Acceleration);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (disableScoring || currentKickData.Result != KickResult.None ) return;
        
        if (other.gameObject.TryGetComponent(out IScoreArea score))
        {
            currentKickData.Result = score.KickResult;
            switch (score.KickResult)
            {
                case KickResult.OutOfBounds:
                    Debug.Log("Out of bounds!");
                    break;
                case KickResult.Goal:
                    Debug.Log("Goal scored!");
                    break;
                case KickResult.Point:
                    Debug.Log("Point scored!");
                    break;
            }
            // delay check to ensure ball didn't hit post shortly after entering trigger
            StartCoroutine(DelayCheckForScore(0.05f));
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (currentKickData.Result == KickResult.OutOfBounds || disableScoring) return;
        
        if(collision.gameObject.TryGetComponent(out IGoalPost goalPost))
        {
            switch(goalPost.GoalPostType)
            {
                case GoalPostType.Goal:
                    currentKickData.Result = KickResult.HitGoalPost;
                    AudioController.Instance.PlaySFX(AudioId.BallHitGoalPost);
                    break;
                case GoalPostType.Point:
                    currentKickData.Result = KickResult.HitPointPost;
                    AudioController.Instance.PlaySFX(AudioId.BallHitGoalPost);
                    break;
                case GoalPostType.None: default:
                    Debug.Log("Goal post is none!");
                    break;
            }

            BallScored();
        }
    }

    public void DeactivateRigidbody()
    {
        if(ballRigidbody.isKinematic == true) return;
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

    IEnumerator DelayCheckForScore(float delay)
    {
        yield return new WaitForSeconds(delay);
        if(currentKickData.Result != KickResult.HitGoalPost && currentKickData.Result != KickResult.HitPointPost)
        {
            BallScored();
        }
    }

    private IEnumerator WaitForBallToStop()
    {
        float elapsedTime = 0f;

        while (ballRigidbody.linearVelocity.magnitude >= minBallSpeed && elapsedTime < maxBallLifeTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null; // Wait for the next frame
        }
        // Invoke the scoring event after the loop exits
        BallScored();
    }

    private void BallScored()
    {
        // Invoke the scoring event after the loop exits
        Debug.Log("BallScored!");
        OnKickComplete?.Invoke(currentKickData);
        disableScoring = true;
        if(poolMember != null)
        {
            poolMember.ReturnToPool(3f);
        }
        else
        {
            Debug.LogWarning("Pool member is null!");
        }
        StopAllCoroutines();
        SetWindActive(false);
    }
}

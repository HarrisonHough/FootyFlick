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
    private Vector3 windDirection = new Vector3(1,0,0);
    private bool windActive;
    
    private bool disableScoring;
    
    private BallScoreData _ballScoreData;
    
    public static Action<BallScoreData> OnScore;
    
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
        _ballScoreData = new BallScoreData();
        ActivateRigidbody();
        ballRigidbody.linearVelocity = velocity;
        ballRigidbody.angularVelocity = transform.right * spinSpeed; // Backward spin
        StartCoroutine(WaitForBallToStop());
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

    private void OnTriggerEnter(Collider other)
    {
        if (_ballScoreData.scoreType != ScoreType.None || _ballScoreData.goalPostCollisionType != GoalPostType.None) return;
        if (other.gameObject.TryGetComponent(out IScoreArea score))
        {
            _ballScoreData.scoreType = score.ScoreType;
            switch (score.ScoreType)
            {
                case ScoreType.OutOfBounds:
                    Debug.Log("Out of bounds!");
                    break;
                case ScoreType.Goal:
                    Debug.Log("Goal scored!");
                    break;
                case ScoreType.Point:
                    Debug.Log("Point scored!");
                    break;
            }
            // delay check to ensure ball didn't hit post shortly after entering trigger
            StartCoroutine(DelayCheckForScore(0.2f));
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        SetWindActive(false);
        if(collision.gameObject.TryGetComponent(out IGoalPost goalPost))
        {
            _ballScoreData.goalPostCollisionType = goalPost.GoalPostType;
            switch(goalPost.GoalPostType)
            {
                case GoalPostType.Goal:
                    _ballScoreData.scoreType = ScoreType.Point;
                    OnScore?.Invoke(_ballScoreData);
                    StopAllCoroutines();
                    break;
                case GoalPostType.Point:
                    _ballScoreData.scoreType = ScoreType.OutOfBounds;
                    OnScore?.Invoke(_ballScoreData);
                    StopAllCoroutines();
                    break;
                case GoalPostType.None: default:
                    Debug.Log("Goal post is none!");
                    break;
            }
        }
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

    IEnumerator DelayCheckForScore(float delay)
    {
        yield return new WaitForSeconds(delay);
        OnScore?.Invoke(_ballScoreData);
        disableScoring = false;
        StopAllCoroutines();
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
        OnScore?.Invoke(_ballScoreData);
        disableScoring = false;
    }
}

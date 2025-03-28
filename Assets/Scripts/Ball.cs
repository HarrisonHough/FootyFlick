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
    
    public static Action<BallScoreData> OnBallScoreComplete;
    private PoolMember poolMember;

    private void Start()
    {
        GameController.OnGameOver += Reset;
    }

    private void OnDestroy()
    {
        GameController.OnGameOver -= Reset;
    }

    private void OnEnable()
    {
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

    private void Reset()
    {
        SetWindActive(false);
        StopAllCoroutines();
        poolMember.ReturnToPool(0);
    }
    
    public void SetWindActive(bool active)
    {
        windActive = active;
    }

    public void LaunchBall(Vector3 velocity)
    {
        if(poolMember == null)
        {
            poolMember = GetComponent<PoolMember>();
        }
        disableScoring = false;
        _ballScoreData = new BallScoreData();
        ActivateRigidbody();
        ballRigidbody.linearVelocity = velocity;
        ballRigidbody.angularVelocity = transform.right * spinSpeed; // Backward spin
        SetWindActive(true);
        windStrength = WindControl.Instance.WindForce;
        windDirection = WindControl.Instance.WindDirection;
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
        if (disableScoring || _ballScoreData.scoreType != ScoreType.None || _ballScoreData.goalPostCollisionType != GoalPostType.None) return;
        
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
            StartCoroutine(DelayCheckForScore(0.05f));
            
        }
    }
    
    private void OnCollisionEnter(Collision collision)
    {
        if (_ballScoreData.scoreType == ScoreType.OutOfBounds || disableScoring) return;
        
        if(collision.gameObject.TryGetComponent(out IGoalPost goalPost))
        {
            _ballScoreData.goalPostCollisionType = goalPost.GoalPostType;
            switch(goalPost.GoalPostType)
            {
                case GoalPostType.Goal:
                    _ballScoreData.scoreType = ScoreType.Point;
                    break;
                case GoalPostType.Point:
                    _ballScoreData.scoreType = ScoreType.OutOfBounds;
                    break;
                case GoalPostType.None: default:
                    Debug.Log("Goal post is none!");
                    break;
            }
            OnBallScoreComplete?.Invoke(_ballScoreData);
            disableScoring = true;
            Handheld.Vibrate();
            SetWindActive(false);
            StopAllCoroutines();
            poolMember.ReturnToPool(1);
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
        if(_ballScoreData.goalPostCollisionType == GoalPostType.None)
        {
            OnBallScoreComplete?.Invoke(_ballScoreData);
            Handheld.Vibrate();
            disableScoring = true;
            poolMember.ReturnToPool(1);
            StopAllCoroutines();
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
        Handheld.Vibrate();
        // Invoke the scoring event after the loop exits
        OnBallScoreComplete?.Invoke(_ballScoreData);
        disableScoring = true;
        poolMember.ReturnToPool(1);
    }
}

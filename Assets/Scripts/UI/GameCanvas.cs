using System;
using System.Collections;
using TMPro;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI notificationText;
    [SerializeField]
    private TextMeshProUGUI scoreText;

    private void Start()
    {
        Ball.OnBallScoreComplete += OnScore;
        PlayerScore.OnScoreUpdated += OnScoreUpdated;
        GameController.OnGameOver += OnGameOver;
        GameController.OnGameStart += OnGameStart;
    }

    public void OnDestroy()
    {
        Ball.OnBallScoreComplete -= OnScore;
        PlayerScore.OnScoreUpdated -= OnScoreUpdated;
        GameController.OnGameOver -= OnGameOver;
    }
    private void OnGameOver()
    {
        gameObject.SetActive(false);
    }
    
    private void OnGameStart()
    {
        ResetUI();
    }

    private void OnScoreUpdated(PlayerScoreData playerScoreData)
    {
        scoreText.text = playerScoreData.score.ToString();
    }

    public void OnScore(BallScoreData ballScoreData)
    {
        if(notificationText == null)
        {
            Debug.LogWarning($"Notification text is not set on {gameObject.name}");
            return;
        }
        
        notificationText.text = "";

        
        switch (ballScoreData.scoreType)
        {
            case ScoreType.Goal:
                notificationText.text = "GOAL!";
                break;
            case ScoreType.OutOfBounds: 
                notificationText.text = "OUT OF BOUNDS!";
                break;
            case ScoreType.Point:
                notificationText.text = "POINT!";
                break;
            case ScoreType.None:
                notificationText.text = "NO SCORE!";
                break;
        }

        switch (ballScoreData.goalPostCollisionType)
        {
            case GoalPostType.Goal:
                notificationText.text = "HIT THE POST!";
                break;
            case GoalPostType.Point:
                notificationText.text = "OUT OF BOUNDS!";
                break;
        }
        StopAllCoroutines();
        StartCoroutine(DelayClearNotificationText(2f));
    }
    
    private void ResetUI()
    {
        notificationText.text = "";
        scoreText.text = "0";
    }
    
    IEnumerator DelayClearNotificationText(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationText.text = "";
    }
}

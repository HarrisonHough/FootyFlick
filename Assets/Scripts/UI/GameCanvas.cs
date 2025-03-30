using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameCanvas : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI notificationText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private WindPanel windPanel;

    private void Start()
    {
        Ball.OnBallScoreComplete += OnScore;
        PlayerScore.OnScoreUpdated += OnScoreUpdated;
        GameManager.OnGameOver += OnGameOver;
        GameManager.OnGameStart += OnGameStart;
    }

    public void OnDestroy()
    {
        Ball.OnBallScoreComplete -= OnScore;
        PlayerScore.OnScoreUpdated -= OnScoreUpdated;
        GameManager.OnGameOver -= OnGameOver;
        GameManager.OnGameStart -= OnGameStart;
    }
    private void OnGameOver()
    {
        SetUIVisibility(false);
    }

    private void SetUIVisibility( bool isVisible)
    {
        windPanel.gameObject.SetActive(isVisible);
        notificationText.gameObject.SetActive(isVisible);
        scoreText.gameObject.SetActive(isVisible);
    }
    
    private void ShowUI()
    {
        windPanel.gameObject.SetActive(false);
        notificationText.gameObject.SetActive(false);
        scoreText.gameObject.SetActive(false);
    }

    private void OnGameStart()
    {
        ResetUI();
    }

    private void OnScoreUpdated(PlayerScoreData playerScoreData)
    {
        if (playerScoreData.score == 0)
        {
            scoreText.text = playerScoreData.score.ToString();
            return;
        }
        scoreText.rectTransform.DOScale( new Vector3(1.2f, 1.2f, 1.2f), 0.3f).OnComplete(() =>
        {
            scoreText.text = playerScoreData.score.ToString();
            scoreText.rectTransform.DOScale( new Vector3(1f, 1f, 1f), 0.2f);
        });
    }

    public void OnScore(BallScoreData ballScoreData)
    {
        if(notificationText == null)
        {
            Debug.LogWarning($"Notification text is not set on {gameObject.name}");
            return;
        }
        
        notificationText.text = "";

        
        switch (ballScoreData.kickResult)
        {
            case KickResult.Goal:
                notificationText.text = "GOAL!";
                break;
            case KickResult.OutOfBounds: 
                notificationText.text = "OUT!";
                break;
            case KickResult.Point:
                notificationText.text = "POINT!";
                break;
            case KickResult.None:
                notificationText.text = "NO SCORE!";
                break;
        }

        switch (ballScoreData.goalPostCollisionType)
        {
            case GoalPostType.Goal:
                notificationText.text = "HIT THE POST!";
                break;
            case GoalPostType.Point:
                notificationText.text = "OUT!";
                break;
        }
        notificationText.rectTransform.DOScale( new Vector3(1.2f, 1.2f, 1.2f), 0.3f).OnComplete(() =>
        {
            notificationText.rectTransform.DOScale( new Vector3(1f, 1f, 1f), 0.2f);
        });
        StopAllCoroutines();
        StartCoroutine(DelayClearNotificationText(2f));
    }
    
    private void ResetUI()
    {
        SetUIVisibility(true);
        notificationText.text = "";
        scoreText.text = "0";
    }
    
    IEnumerator DelayClearNotificationText(float delay)
    {
        yield return new WaitForSeconds(delay);
        notificationText.text = "";
    }
}

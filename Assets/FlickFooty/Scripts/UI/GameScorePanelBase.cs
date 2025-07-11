using System.Collections;
using DG.Tweening;
using TMPro;
using UnityEngine;

public class GameScorePanelBase : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI feedbackText;
    [SerializeField]
    private TextMeshProUGUI scoreText;
    
    private void Start()
    {
        Ball.OnKickComplete += OnKickComplete;
        PlayerScore.OnScoreUpdated += OnScoreUpdated;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    public void OnDestroy()
    {
        Ball.OnKickComplete -= OnKickComplete;
        PlayerScore.OnScoreUpdated -= OnScoreUpdated;
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
    
    private void SetUIVisibility(bool isVisible)
    {
        feedbackText.gameObject.SetActive(isVisible);
        scoreText.gameObject.SetActive(isVisible);
    }
    
    protected virtual void OnGameStateChanged(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.GameStarted:
                ResetUI();
                break;
            case GameStateEnum.GameOver:
                SetUIVisibility(false);
                break;
            default:
                break;
        }
    }

    protected virtual void OnScoreUpdated(PlayerScoreData playerScoreData)
    {
        if (playerScoreData.Score == 0)
        {
            scoreText.text = playerScoreData.Score.ToString();
            return;
        }
        scoreText.rectTransform.DOScale( new Vector3(1.2f, 1.2f, 1.2f), 0.3f).OnComplete(() =>
        {
            scoreText.text = playerScoreData.Score.ToString();
            scoreText.rectTransform.DOScale( new Vector3(1f, 1f, 1f), 0.2f);
        });
    }

    protected virtual void OnKickComplete(KickData kickData)
    {
        if(feedbackText == null)
        {
            Debug.LogWarning($"Notification text is not set on {gameObject.name}");
            return;
        }
        
        feedbackText.text = "";

        
        switch (kickData.Result)
        {
            case KickResult.Goal:
                feedbackText.text = "GOAL!";
                break;
            case KickResult.OutOfBounds: 
                feedbackText.text = "OUT!";
                break;
            case KickResult.Point:
                feedbackText.text = "POINT!";
                break;
            case KickResult.None:
                feedbackText.text = "NO SCORE!";
                break;
            case KickResult.HitGoalPost:
                feedbackText.text = "HIT THE POST!";
                break;
            case KickResult.HitPointPost:
                feedbackText.text = "OUT ON THE FULL!";
                break;
            case KickResult.BinGoal:
                feedbackText.text = "SWISH!";
                break;
        }
        
        feedbackText.rectTransform.DOScale( new Vector3(1.2f, 1.2f, 1.2f), 0.3f).OnComplete(() =>
        {
            feedbackText.rectTransform.DOScale( new Vector3(1f, 1f, 1f), 0.2f);
        });
        StopAllCoroutines();
        StartCoroutine(DelayClearNotificationText(2f));
    }
    
    private void ResetUI()
    {
        SetUIVisibility(true);
        feedbackText.text = "";
        scoreText.text = "0";
    }
    
    IEnumerator DelayClearNotificationText(float delay)
    {
        yield return new WaitForSeconds(delay);
        feedbackText.text = "";
    }
}

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
        Ball.OnKickComplete += OnScore;
        PlayerScore.OnScoreUpdated += OnScoreUpdated;
        GameManager.OnGameStateChanged += OnGameStateChanged;
    }

    private void OnGameStateChanged(GameStateEnum gameState)
    {
        switch (gameState)
        {
            case GameStateEnum.GameStarted:
                OnGameStart();
                break;
            case GameStateEnum.GameOver:
                OnGameOver();
                break;
            default:
                break;
        }
    }

    public void OnDestroy()
    {
        Ball.OnKickComplete -= OnScore;
        PlayerScore.OnScoreUpdated -= OnScoreUpdated;
        GameManager.OnGameStateChanged -= OnGameStateChanged;
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
    
    public void HideWindPanel()
    {
        windPanel.gameObject.SetActive(false);
    }
    
    public void ShowWindPanel()
    {
        windPanel.gameObject.SetActive(true);
    }

    private void OnGameStart()
    {
        ResetUI();
    }

    private void OnScoreUpdated(PlayerScoreData playerScoreData)
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

    public void OnScore(KickData kickData)
    {
        if(notificationText == null)
        {
            Debug.LogWarning($"Notification text is not set on {gameObject.name}");
            return;
        }
        
        notificationText.text = "";

        
        switch (kickData.Result)
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
            case KickResult.HitGoalPost:
                notificationText.text = "HIT THE POST!";
                break;
            case KickResult.HitPointPost:
                notificationText.text = "OUT ON THE FULL!";
                break;
            case KickResult.BinGoal:
                notificationText.text = "SWISH!";
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
        Debug.Log("Resetting UI");
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

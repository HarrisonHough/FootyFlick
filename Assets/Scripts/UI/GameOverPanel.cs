using System;
using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;

    public Action OnRetryButtonClicked;
    public Action OnHomeButtonClicked;
    
    public void UpdateText(PlayerScoreData scoreData)
    {
        var score = scoreData.Score;
        var bestScore = GamePrefs.GetBestScore(GameModeEnum.GoalOrNothing);
        if(score > bestScore)
        {
            bestScore = score;
            GamePrefs.SetBestScore(GameModeEnum.GoalOrNothing, bestScore);
        }
        scoreText.text = score.ToString();
        // TODO - Add best score to UI with some NEW text or something
        bestScoreText.text = bestScore.ToString();
    }

    public void Retry()
    {
        OnRetryButtonClicked?.Invoke();
    }
    
    public void Home()
    {
        OnHomeButtonClicked?.Invoke();
    }
}

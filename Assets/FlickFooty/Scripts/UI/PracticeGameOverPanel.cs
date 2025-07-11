using TMPro;
using UnityEngine;

public class PracticeGameOverPanel : GameOverPanelBase
{
    [SerializeField]
    private TMP_Text scoreText;
    [SerializeField]
    private TMP_Text accuracyText;
    
    public void UpdateText(PlayerScoreData scoreData)
    {
        scoreText.text = $"{scoreData.GoalCount.ToString()}.{scoreData.PointCount.ToString()} {scoreData.Score.ToString()}";
        accuracyText.text = scoreData.Accuracy.ToString("0") + "%";
        
        var score = scoreData.Score;
        var bestScore = GamePrefs.GetBestScore(GameModeEnum.Practice);
        Debug.Log("Best Score: " + bestScore);
        Debug.Log("Score: " + score);
        if(score > bestScore)
        {
            Debug.Log("New Best Score: " + score);
            bestScore = score;
            GamePrefs.SetBestScore(GameModeEnum.Practice, bestScore);
        }
    }
}

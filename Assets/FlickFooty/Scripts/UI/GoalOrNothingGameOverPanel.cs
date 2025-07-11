using TMPro;
using UnityEngine;

public class GoalOrNothingGameOverPanel : GameOverPanelBase
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    
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
}

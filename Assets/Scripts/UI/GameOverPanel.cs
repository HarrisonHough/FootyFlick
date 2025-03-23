using TMPro;
using UnityEngine;

public class GameOverPanel : MonoBehaviour
{
    [SerializeField]
    private TextMeshProUGUI scoreText;
    [SerializeField]
    private TextMeshProUGUI bestScoreText;
    [SerializeField]
    private PlayerScore playerScore;
    
    private void OnEnable()
    {
        UpdateText();
    }
    
    public void UpdateText()
    {
        if(playerScore == null)
        {
            Debug.LogWarning("Player score is not set on GameOverPanel");
            return;
        }
        var score = playerScore.GetScoreData.score;
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

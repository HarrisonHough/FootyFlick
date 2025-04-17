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
    }
}

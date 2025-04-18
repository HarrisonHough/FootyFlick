using TMPro;
using UnityEngine;

public class TimeAttackGameScorePanel : GameScorePanelBase
{
    [SerializeField] TMP_Text timeText;
    
    public void UpdateTimer(float time)
    {
        timeText.text = time.ToString("0");
    }
}

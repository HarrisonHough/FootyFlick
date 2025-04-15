using UnityEngine;

public class PracticeMode : GameModeBase
{
    private int tutorialStep = 0;
    private bool gameOver = false;
    [SerializeField]
    private TutorialPanel tutorialPanel;
    
    public override void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        Ball.OnKickComplete += OnKickResult;
    }

    public override void StartMode()
    {
        GameManager.SetGameState(GameStateEnum.GameStarted);
        //gameCanvas = gameManager.GetGameCanvas();
        //gameCanvas.HideWindPanel();
        WindControl.Instance.RandomizeWindStrength();
        tutorialStep = 0;
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }

    public override void OnKickResult(KickData kickData)
    {
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }

    public override void EndMode()
    {
        Ball.OnKickComplete -= OnKickResult;
        tutorialPanel.OnTutorialComplete -= EndMode;
        if (tutorialPanel != null)
        {
            Destroy(tutorialPanel.gameObject);
        }
        tutorialPanel = null;
    }

    private void ShowTutorialComplete()
    {
        gameOver = true;
    }
}

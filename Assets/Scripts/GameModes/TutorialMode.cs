using UnityEngine;

public class TutorialMode : GameModeBase
{
    [SerializeField] private TutorialPanel tutorialPanelPrefab;
    
    private int tutorialStep = 0;
    private bool gameOver = false;
    private TutorialPanel tutorialPanel;
    
    public override void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        var gameCanvas = gameManager.GetGameCanvas();
        tutorialPanel = Instantiate(tutorialPanelPrefab, gameCanvas.transform);
    }

    public override void StartMode()
    {
        tutorialStep = 0;
        StartStep0();
    }

    public override void OnKickResult(KickData kickData)
    {
        switch (tutorialStep)
        {
            case 0:
                if (kickData.Result == KickResult.Goal)
                {
                    tutorialStep++;
                    StartStep1();
                }
                break;
            case 1:
                if (kickData.Result == KickResult.Goal)
                {
                    tutorialStep++;
                    StartStep2();
                }
                break;
            case 2:
                if (kickData.Result == KickResult.Goal && kickData.Style != KickStyle.DropPunt)
                {
                    FinishTutorial();
                }
                break;
        }
    }

    public override void EndMode()
    {
        if (tutorialPanel != null)
        {
            Destroy(tutorialPanel.gameObject);
        }
        tutorialPanel = null;
    }

    private void StartStep0()
    {
        WindControl.Instance.SetWindStrength(0);
        tutorialPanel.ShowKickTutorial();
        //game.SetPlayerPosition(TutorialPositions.Easy);
        //game.ui.ShowTutorialPanel("Swipe to kick!");
    }

    private void StartStep1()
    {
        WindControl.Instance.SetWindStrength(1);
        tutorialPanel.ShowWindTutorial();
        //game.SetPlayerPosition(TutorialPositions.Center);
        //game.ui.ShowTutorialPanel("Wind affects the ball. Swipe slightly right to compensate.");
    }

    private void StartStep2()
    {
        WindControl.Instance.SetWindStrength(0);
        tutorialPanel.ShowKickStyleTutorial();
        //game.SetPlayerPosition(TutorialPositions.TightAngle);
        //game.ui.ShowTutorialPanel("Swipe down-right to change to snap kick. Try curving it through!");
    }

    private void FinishTutorial()
    {
        //game.ui.ShowTutorialPanel("You're ready! Let's play.");
        gameOver = true;
    }
}

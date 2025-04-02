using System.Collections;
using UnityEngine;

public class TutorialMode : GameModeBase
{
    [SerializeField] private TutorialPanel tutorialPanelPrefab;
    
    private int tutorialStep = 0;
    private bool gameOver = false;
    private TutorialPanel tutorialPanel;
    private GameCanvas gameCanvas;
    
    public override void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        gameCanvas = gameManager.GetGameCanvas();
        tutorialPanel = Instantiate(tutorialPanelPrefab, gameCanvas.transform);
        Ball.OnKickComplete += OnKickResult;
    }

    public override void StartMode()
    {
        gameCanvas = gameManager.GetGameCanvas();
        gameCanvas.HideWindPanel();
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
                    StartCoroutine(DelayStep1(1f));
                }
                break;
            case 1:
                if (kickData.Result == KickResult.Goal)
                {
                    tutorialStep++;
                    StartCoroutine(DelayStep2(1f));;
                }
                break;
            case 2:
                if (kickData.Result == KickResult.Goal && kickData.Style != KickStyle.DropPunt)
                {
                    FinishTutorial();
                }
                break;
        }
        
        if(kickData.Result != KickResult.Goal)
        {
            GameManager.SetGameState(GameStateEnum.GameKicking);
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
        GameManager.SetGameState(GameStateEnum.GameKicking);
        //game.SetPlayerPosition(TutorialPositions.Easy);
        //game.ui.ShowTutorialPanel("Swipe to kick!");
    }
    
    private IEnumerator DelayStep1(float delay)
    {
        yield return new WaitForSeconds(delay);
        WindControl.Instance.SetWindStrength(-1);
        tutorialPanel.ShowWindTutorial();
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }

    private IEnumerator DelayStep2(float delay)
    {
        yield return new WaitForSeconds(delay);
        WindControl.Instance.SetWindStrength(0);
        tutorialPanel.ShowKickStyleTutorial();
        GameManager.SetGameState(GameStateEnum.GameKicking);
        //game.SetPlayerPosition(TutorialPositions.TightAngle);
        //game.ui.ShowTutorialPanel("Swipe down-right to change to snap kick. Try curving it through!");
    }

    private void FinishTutorial()
    {
        tutorialPanel.ShowTutorialComplete();
        gameOver = true;
    }
}

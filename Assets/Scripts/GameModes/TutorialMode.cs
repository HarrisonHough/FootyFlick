public class TutorialMode : IGameMode
{
    private GameManager game;
    private int tutorialStep = 0;
    private bool gameOver = false;

    public TutorialMode(GameManager gm) => game = gm;

    public void StartMode()
    {
        tutorialStep = 0;
        StartStep0();
    }

    public void Update(float deltaTime) { }

    public void OnKickResult(KickResult result)
    {
        switch (tutorialStep)
        {
            case 0:
                if (result == KickResult.Goal)
                {
                    tutorialStep++;
                    StartStep1();
                }
                break;
            case 1:
                if (result == KickResult.Goal)
                {
                    tutorialStep++;
                    StartStep2();
                }
                break;
            case 2:
                if (result == KickResult.Goal && game.CurrentKickStyle != KickStyle.DropPunt)
                {
                    FinishTutorial();
                }
                break;
        }
    }

    public bool IsGameOver => gameOver;

    public string GetStatusText() => "Tutorial"; // You could return step text too

    public void EndMode() { }

    private void StartStep0()
    {
        WindControl.Instance.SetWindStrength(0);
        //game.SetPlayerPosition(TutorialPositions.Easy);
        //game.ui.ShowTutorialPanel("Swipe to kick!");
    }

    private void StartStep1()
    {
        WindControl.Instance.SetWindStrength(1);
        //game.SetPlayerPosition(TutorialPositions.Center);
        //game.ui.ShowTutorialPanel("Wind affects the ball. Swipe slightly right to compensate.");
    }

    private void StartStep2()
    {
        WindControl.Instance.SetWindStrength(0);
        //game.SetPlayerPosition(TutorialPositions.TightAngle);
        //game.ui.ShowTutorialPanel("Swipe down-right to change to snap kick. Try curving it through!");
    }

    private void FinishTutorial()
    {
        //game.ui.ShowTutorialPanel("You're ready! Let's play.");
        gameOver = true;
    }
}

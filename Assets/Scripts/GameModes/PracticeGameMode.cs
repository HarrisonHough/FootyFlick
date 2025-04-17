using System.Collections;
using UnityEngine;

public class PracticeGameMode : GameModeBase
{
    private bool gameOver = false;
    [SerializeField]
    private TutorialPanel tutorialPanel;
    [SerializeField]
    private PracticeGameOverPanel gameOverPanel;
    private PlayerScore playerScore;
    
    public override void Initialize(GameManager gameManager)
    {
        playerScore = gameManager.GetPlayerScore();
        this.gameManager = gameManager;
        Ball.OnKickComplete += OnKickResult;
        gameOverPanel.OnHomeButtonClicked += OnHomeButtonClicked;
        WindControl.Instance.RandomizeWindStrength();
    }

    public void SetPaused(bool paused)
    {
        gameManager.SetSwipeDisabled(paused);
    }

    private void OnHomeButtonClicked()
    {
        gameOverPanel.gameObject.SetActive(false);
        GameManager.SetGameState(GameStateEnum.Home);
    }

    public override void StartMode()
    {
        GameManager.SetGameState(GameStateEnum.GameStarted);
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }

    public override void OnKickResult(KickData kickData)
    {
        playerScore.AddKickData(kickData);
        StartCoroutine(StartNextKick(1f));
    }

    private IEnumerator StartNextKick(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }

    public override void EndMode()
    {
        Ball.OnKickComplete -= OnKickResult;
    }
    
    public void GameOver()
    {
        gameOverPanel.gameObject.SetActive(true);
        gameOver = true;
        GameManager.SetGameState(GameStateEnum.GameOver);
        gameOverPanel.UpdateText(playerScore.GetScoreData);
    }

    private void ShowTutorialComplete()
    {
        gameOver = true;
    }
}

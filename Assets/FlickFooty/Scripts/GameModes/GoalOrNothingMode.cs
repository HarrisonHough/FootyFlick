using System.Collections;
using UnityEngine;

public class GoalOrNothingMode : GameModeBase
{
    [SerializeField]
    private GoalOrNothingGameOverPanel goalOrNothingGameOverPanel;
    [SerializeField]
    private GameScorePanelBase gameScorePanel;
    
    private bool gameOver = false;
    private PlayerScore playerScore;
    
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        playerScore = gameManager.GetPlayerScore();
        goalOrNothingGameOverPanel.gameObject.SetActive(false);
        goalOrNothingGameOverPanel.OnHomeButtonClicked += OnHomeButtonClicked;
        goalOrNothingGameOverPanel.OnRetryButtonClicked += OnRetryButtonClicked;
        Ball.OnKickComplete += OnKickResult;
        WindControl.Instance.RandomizeWindStrength();
    }

    private void OnRetryButtonClicked()
    {
        GameManager.SetGameState(GameStateEnum.GameStarted);
        StartMode();
    }

    private void OnHomeButtonClicked()
    {
        goalOrNothingGameOverPanel.gameObject.SetActive(false);
        GameManager.SetGameState(GameStateEnum.Home);
    }

    public override void StartMode()
    {
        gameCanvasObject.SetActive(true);
        GameManager.SetGameState(GameStateEnum.GameKicking);
        goalOrNothingGameOverPanel.gameObject.SetActive(false);
        gameOver = false;
        gameCanvasObject.SetActive(true);
    }

    public override void OnKickResult(KickData kickData)
    {
        playerScore.AddKickData(kickData);
        if (kickData.Result != KickResult.Goal)
        {
            gameOver = true;
            StopAllCoroutines();
            StartCoroutine( DelayGameOver(1f));
            return;
        }
        StopAllCoroutines();
        StartCoroutine(DelayMovePlayer (1f));
    }

    public override void EndMode()
    {
        goalOrNothingGameOverPanel.gameObject.SetActive(false);
        goalOrNothingGameOverPanel.OnHomeButtonClicked -= OnHomeButtonClicked;
        goalOrNothingGameOverPanel.OnRetryButtonClicked -= OnRetryButtonClicked;
        Ball.OnKickComplete -= OnKickResult;
    }
    
    private IEnumerator DelayMovePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }
    
    private IEnumerator DelayGameOver(float delay)
    {
        yield return new WaitForSeconds(delay);
        GameManager.SetGameState(GameStateEnum.GameOver);
        goalOrNothingGameOverPanel.UpdateText(playerScore.GetScoreData);
        goalOrNothingGameOverPanel.gameObject.SetActive(true);
    }
    
    private void OnDestroy()
    {
        Ball.OnKickComplete -= OnKickResult;
    }
}

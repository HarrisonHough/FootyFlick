using System.Collections;
using UnityEngine;

public class GoalOrNothingMode : GameModeBase
{
    [SerializeField] 
    private GameScorePanelBase gameGameScorePanelPrefab;
    [SerializeField]
    private GoalOrNothingGameOverPanel goalOrNothingGameOverPanelPrefab;

    private bool gameOver = false;

    private GoalOrNothingGameOverPanel goalOrNothingGameOverPanel;
    private GameScorePanelBase gameScorePanel;
    private PlayerScore playerScore;
    
    public override void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        playerScore = gameManager.GetPlayerScore();
        var gameCanvas = gameManager.GetGameCanvas();
        goalOrNothingGameOverPanel = Instantiate(goalOrNothingGameOverPanelPrefab, gameCanvas.transform);
        goalOrNothingGameOverPanel.gameObject.SetActive(false);
        goalOrNothingGameOverPanel.OnHomeButtonClicked += OnHomeButtonClicked;
        goalOrNothingGameOverPanel.OnRetryButtonClicked += OnRetryButtonClicked;
        gameScorePanel = Instantiate(gameGameScorePanelPrefab, gameCanvas.transform);
        Ball.OnKickComplete += OnKickResult;
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
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        GameManager.SetGameState(GameStateEnum.GameKicking);
        goalOrNothingGameOverPanel.gameObject.SetActive(false);
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
        Destroy(goalOrNothingGameOverPanel?.gameObject);
        Destroy(gameScorePanel?.gameObject);
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

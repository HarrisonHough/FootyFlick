using System.Collections;
using UnityEngine;

public class GoalOrNothingMode : GameModeBase
{
    [SerializeField]
    private GameOverPanel gameOverPanelPrefab;

    private bool gameOver = false;

    private GameOverPanel gameOverPanel;
    private PlayerScore playerScore;
    
    public override void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        playerScore = gameManager.GetPlayerScore();
        var gameCanvas = gameManager.GetGameCanvas();
        gameOverPanel = Instantiate(gameOverPanelPrefab, gameCanvas.transform);
        gameOverPanel.gameObject.SetActive(false);
        gameOverPanel.OnHomeButtonClicked += OnHomeButtonClicked;
        gameOverPanel.OnRetryButtonClicked += OnRetryButtonClicked;
        Ball.OnKickComplete += OnKickResult;
    }

    private void OnRetryButtonClicked()
    {
        GameManager.SetGameState(GameStateEnum.GameStarted);
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        GameManager.SetGameState(GameStateEnum.GameKicking);
        gameOverPanel.gameObject.SetActive(false);
    }

    private void OnHomeButtonClicked()
    {
        gameOverPanel.gameObject.SetActive(false);
        GameManager.SetGameState(GameStateEnum.Home);
    }

    public override void StartMode()
    {
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        GameManager.SetGameState(GameStateEnum.GameKicking);
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
        gameOverPanel.gameObject.SetActive(false);
        gameOverPanel.OnHomeButtonClicked -= OnHomeButtonClicked;
        gameOverPanel.OnRetryButtonClicked -= OnRetryButtonClicked;
        Destroy(gameOverPanel?.gameObject);
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
        gameOverPanel.gameObject.SetActive(true);
    }
    
    private void OnDestroy()
    {
        Ball.OnKickComplete -= OnKickResult;
    }
}

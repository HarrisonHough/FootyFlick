using System.Collections;
using UnityEngine;

public class GameModeTimeAttack : GameModeBase
{
    [SerializeField] private TimeAttackGameScorePanel gameScorePanelPrefab;
    [SerializeField] private TimeAttackGameOver gameOverPanelPrefab;
    [SerializeField] private float timeLimit = 60f;

    private float timer;
    private int score;
    private bool gameOver;
    
    private TimeAttackGameScorePanel gameScorePanel;
    private TimeAttackGameOver gameOverUI;
    private PlayerScore playerScore;
    public override void Initialize(GameManager gameManager)
    {
        this.gameManager = gameManager;
        playerScore = gameManager.GetPlayerScore();
        var gameCanvas = gameManager.GetGameCanvas();
        gameScorePanel = Instantiate(gameScorePanelPrefab, gameCanvas.transform);
        gameOverUI = Instantiate(gameOverPanelPrefab, gameCanvas.transform);
        gameOverUI.OnHomeButtonClicked += OnHomeButtonClicked;
        gameOverUI.OnRetryButtonClicked += OnRetryButtonClicked;
        gameOverUI.gameObject.SetActive(false);
        gameOver = true;
        Ball.OnKickComplete += OnKickResult;
    }

    private void OnRetryButtonClicked()
    {
        GameManager.SetGameState(GameStateEnum.GameStarted);
        StartMode();
    }

    private void OnHomeButtonClicked()
    {
        gameOverUI.gameObject.SetActive(false);
        GameManager.SetGameState(GameStateEnum.Home);
    }

    public override void StartMode() 
    {
        timer = timeLimit;
        score = 0;
        gameOver = false;
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        gameScorePanel.gameObject.SetActive(true);
        gameOverUI.gameObject.SetActive(false);
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }

    public override void EndMode()
    {
        gameOverUI.gameObject.SetActive(false);
        gameScorePanel.gameObject.SetActive(false);
        gameOver = true;
        GameManager.SetGameState(GameStateEnum.GameStarted);
        gameOverUI.OnHomeButtonClicked -= OnHomeButtonClicked;
        gameOverUI.OnRetryButtonClicked -= OnRetryButtonClicked;
        Destroy(gameScorePanel?.gameObject);
        Destroy(gameOverUI?.gameObject);
        Ball.OnKickComplete -= OnKickResult;
    }

    private void Update()
    {
        if (gameOver) return;

        timer -= Time.deltaTime;
        gameScorePanel.UpdateTimer(timer);

        if (!gameOver && timer <= 0f)
        {
            gameOver = true;
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOverUI.UpdateText(playerScore.GetScoreData);
        gameOverUI.gameObject.SetActive(true);
        GameManager.SetGameState(GameStateEnum.GameOver);
    }

    public override void OnKickResult(KickData kickData)
    {
        playerScore.AddKickData(kickData);
        if (kickData.Result == KickResult.Goal)
            score += 6;
        else if (kickData.Result == KickResult.Point)
            score += 1;
        StartCoroutine(DelayMovePlayer (1f));
    }
    
    private IEnumerator DelayMovePlayer(float delay)
    {
        yield return new WaitForSeconds(delay);
        gameManager.MovePlayerToRandomPosition();
        WindControl.Instance.RandomizeWindStrength();
        GameManager.SetGameState(GameStateEnum.GameKicking);
    }
}
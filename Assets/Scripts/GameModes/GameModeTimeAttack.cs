using System.Collections;
using UnityEngine;

public class GameModeTimeAttack : GameModeBase
{
    [SerializeField]
    private TimeAttackGameScorePanel gameScorePanel;
    [SerializeField]
    private TimeAttackGameOver gameOverUI;
    [SerializeField] private float timeLimit = 60f;

    private float timer;
    private int lastDisplayedSeconds = -1;
    private int score;
    private bool gameOver;
    private PlayerScore playerScore;
    private bool pauseTimer;
    
    public override void Initialize(GameManager gameManager)
    {
        base.Initialize(gameManager);
        playerScore = gameManager.GetPlayerScore();
        gameOverUI.OnHomeButtonClicked += OnHomeButtonClicked;
        gameOverUI.OnRetryButtonClicked += OnRetryButtonClicked;
        gameOverUI.gameObject.SetActive(false);
        pauseTimer = true;
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
        gameOverUI.gameObject.SetActive(false);
        gameScorePanel.gameObject.SetActive(false);
        GameManager.SetGameState(GameStateEnum.Home);
    }

    public override void StartMode() 
    {
        GameManager.SetGameState(GameStateEnum.GameKicking);
        gameCanvasObject.SetActive(true);
        timer = timeLimit;
        score = 0;
        gameOver = false;
        pauseTimer = false;
        if(!GamePrefs.GetTutorialComplete(gameMode))
        {
            SetPaused(true);
            return;
        }
        gameCanvasObject.SetActive(true);
        gameScorePanel.gameObject.SetActive(true);
        gameOverUI.gameObject.SetActive(false);

    }
    
    public override void SetPaused(bool paused)
    {
        base.SetPaused(paused);
        pauseTimer = paused;
    }

    public override void EndMode()
    {
        gameOverUI.gameObject.SetActive(false);
        gameScorePanel.gameObject.SetActive(false);
        gameOver = true;
        GameManager.SetGameState(GameStateEnum.GameStarted);
        gameOverUI.OnHomeButtonClicked -= OnHomeButtonClicked;
        gameOverUI.OnRetryButtonClicked -= OnRetryButtonClicked;
        Destroy(gameScorePanel.gameObject);
        Destroy(gameOverUI.gameObject);
        Ball.OnKickComplete -= OnKickResult;
    }

    private void Update()
    {
        if (pauseTimer) return;

        timer -= Time.deltaTime;

        var currentSeconds = Mathf.CeilToInt(timer); 
        if (currentSeconds != lastDisplayedSeconds)
        {
            lastDisplayedSeconds = currentSeconds;
            gameScorePanel.UpdateTimer(timer); 
        }

        if (!gameOver && timer <= 0f)
        {
            GameOver();
        }
    }

    private void GameOver()
    {
        gameOver = true;
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